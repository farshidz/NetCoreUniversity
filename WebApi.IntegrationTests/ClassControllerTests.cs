using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using NetCoreUniversity.Data;
using NetCoreUniversity.Data.Models;
using Xunit;

namespace NetCoreUniversity.WebApi.IntegrationTests {
    public class ClassControllerTests {
        private readonly HttpClient client;

        public ClassControllerTests() {
            var server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            this.client = server.CreateClient();
        }

        [Fact]
        public async Task Get_ReturnsAllClasses() {
            // Get a fresh test db
            var context = this.GetFreshDbContext();

            var response = await this.client.GetAsync("api/class");
            response.EnsureSuccessStatusCode();

            // Doesn't throw exception unless malformated JSON
            var classes = JsonConvert.DeserializeObject<ICollection<Class>>(
                await response.Content.ReadAsStringAsync()
            );

            Assert.Equal(context.Classes.ToList().Count, classes.Count);
        }

        [Fact]
        public async Task Get_ReturnsClass_WhenFound() {
            // Get a fresh test db
            var context = this.GetFreshDbContext();

            var testClass = context.Classes.First();
            int classId = testClass.ClassId;

            var response = await this.client.GetAsync($"api/class/{classId}");
            response.EnsureSuccessStatusCode();

            var returnedObject = JsonConvert.DeserializeObject<Class>(
                await response.Content.ReadAsStringAsync()
            );

            // Just check two properties
            // TODO: write a method that compares all properties using reflection
            Assert.Equal(testClass.ClassId, returnedObject.ClassId);
            Assert.Equal(testClass.Name, returnedObject.Name);
        }

        [Fact]
        public async Task Create_Inserts_WhenModelValid() {
            // Get a fresh test db
            var context = this.GetFreshDbContext();

            string className = Guid.NewGuid().ToString();
            var testClass = new Class() {
                Location = "1-131 Wellington Road Clayton",
                Name = className
            };

            string jsonClass = this.JsonSerialize(testClass);
            var httpContent = new StringContent(
                jsonClass, Encoding.UTF8, "application/json"
            );
            var response = await this.client.PostAsync(
                $"api/class", httpContent
            );
            response.EnsureSuccessStatusCode();

            // Get the returned object
            var returnedObject = JsonConvert.DeserializeObject<Class>(
                await response.Content.ReadAsStringAsync()
            );

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.NotEqual(0, returnedObject.ClassId);
            Assert.NotNull(
                context.Classes.SingleOrDefault(
                    cls => cls.ClassId == returnedObject.ClassId
                            && cls.Name == className
                )
            );
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenModelInvalid() {
            var testClass = new Class(); // leave all properties null/default

            string jsonClass = this.JsonSerialize(testClass);
            var httpContent = new StringContent(
                jsonClass, Encoding.UTF8, "application/json"
            );
            var response = await this.client.PostAsync(
                $"api/class", httpContent
            );

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        private string JsonSerialize<T>(T obj) {
            return JsonConvert.SerializeObject(obj,
                new JsonSerializerSettings() {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }
            );
        }

        /// <summary>
        /// Gets a SchoolContext for a fresh test DB.
        /// </summary>
        /// <returns>An instance of SchoolContext class.</returns>
        private SchoolContext GetFreshDbContext() {
            var factory = new DbContextFactory();
            factory.Reset();
            return factory.GetContext();
        }

    }
}
