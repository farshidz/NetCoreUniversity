using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using NetCoreUniversity.Data;
using NetCoreUniversity.Data.Models;
using NetCoreUniversity.WebApi.Controllers;

namespace NetCoreUniversity.WebApi.UnitTests {
    public class ClassControllerTests {

        #region Tests

        [Fact]
        public async Task GetAll_ReturnsAllClasses() {
            var allClasses = this.GetTestClasses();
            // Create mock repo
            var mockClassRepo = new Mock<IClassRepository>();
            mockClassRepo
                .Setup(repo => repo.GetAllAsync())
                .Returns(Task.FromResult(allClasses));
            var mockRepos = new Mock<IDataRepositories>();
            mockRepos
                .Setup(repos => repos.ClassRepository())
                .Returns(mockClassRepo.Object);

            var controller = new ClassController(mockRepos.Object);

            var result = await controller.GetAll();

            Assert.NotNull(result);
            Assert.Equal(allClasses.Count(), result.Count());
        }

        [Fact]
        public async Task Get_ReturnsClass_WhenFound() {
            const int id = 2;
            var mockClassRepo = new Mock<IClassRepository>();
            mockClassRepo
                .Setup(repo => repo.GetByIdAsync(id))
                .Returns(Task.FromResult(new Class()));
            var mockRepos = new Mock<IDataRepositories>();
            mockRepos
                .Setup(repos => repos.ClassRepository())
                .Returns(mockClassRepo.Object);

            var controller = new ClassController(mockRepos.Object);

            var result = await controller.Get(id);

            Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<Class>(((OkObjectResult)result).Value);
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenNotFound() {
            const int id = 2;
            var mockClassRepo = new Mock<IClassRepository>();
            mockClassRepo
                .Setup(repo => repo.GetByIdAsync(id))
                .Returns(Task.FromResult(new Class()));
            var mockRepos = new Mock<IDataRepositories>();
            mockRepos
                .Setup(repos => repos.ClassRepository())
                .Returns(mockClassRepo.Object);

            var controller = new ClassController(mockRepos.Object);

            var result = await controller.Get(id + 1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_Inserts_WhenModelValid() {
            var mockClassRepo = new Mock<IClassRepository>();
            mockClassRepo
                .Setup(repo => repo.Insert(It.IsAny<Class>()))
                .Verifiable();
            var mockRepos = new Mock<IDataRepositories>();
            mockRepos
                .Setup(repos => repos.ClassRepository())
                .Returns(mockClassRepo.Object);
            mockRepos
                .Setup(repos => repos.SaveAsync())
                .Returns(Task.CompletedTask)
                .Verifiable();

            var controller = new ClassController(mockRepos.Object);
            var c = new Class() {
                ClassId = 1, // controller expected to set it to zero
                Location = "1-131 Wellington Road Clayton",
                Name = "Class One",
                Teacher = this.GetTestTeacher()
            };

            var result = (await controller.Create(c)) as CreatedAtActionResult;

            Assert.NotNull(result);
            Assert.Equal(result.ActionName, "Get");
            Assert.NotNull(result.RouteValues);
            Assert.IsAssignableFrom<Class>(result.Value);
            mockClassRepo.Verify(); // verify insert call
            mockRepos.Verify(); // verify save call
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenModelInvalid() {
            var mockClassRepo = new Mock<IClassRepository>();
            mockClassRepo
                .Setup(repo => repo.Insert(It.IsAny<Class>()))
                .Throws(new Exception("Must not be called"));
            var mockRepos = new Mock<IDataRepositories>();
            mockRepos
                .Setup(repos => repos.ClassRepository())
                .Returns(mockClassRepo.Object);
            mockRepos
                .Setup(repos => repos.SaveAsync())
                .Throws(new Exception("Must not be called"));

            var controller = new ClassController(mockRepos.Object);
            var c = new Class();
            controller.ModelState.AddModelError("", "error");

            var result = (await controller.Create(c)) as BadRequestObjectResult;

            Assert.NotNull(result);
        }

        #endregion
        #region Test Data Generation

        private ICollection<Class> GetTestClasses() {
            var teacher = this.GetTestTeacher();
            var classes = new List<Class>
            {
                new Class()
                {
                    ClassId = 1,
                    Location = "1-131 Wellington Road Clayton",
                    Name = "Class One",
                    Teacher = this.GetTestTeacher()
                },
                new Class()
                {
                    ClassId = 2,
                    Location = "1-131 Wellington Road Clayton",
                    Name = "Class Two",
                    Teacher = this.GetTestTeacher()
                }
            };
            var students = this.GetTestStudents();
            // Assign students
            foreach (var c in classes) {
                c.Students = students;
            }
            // Assign classes to teacher
            teacher.Classes = new List<Class>()
            {
                classes[0],
                classes[1]
            };

            return classes;
        }

        private ICollection<Student> GetTestStudents() {
            var students = new List<Student>
            {
                new Student()
                {
                    Dob = DateTime.Now.AddYears(-18),
                    GivenName = "John",
                    Surname = "StudentOne",
                    Title = "Mr",
                    Gpa = 3.5F,
                    PersonId = 1
                },
                new Student()
                {
                    Dob = DateTime.Now.AddYears(-20),
                    GivenName = "Marry",
                    Surname = "StudentTwo",
                    Title = "Ms",
                    Gpa = 2.1F,
                    PersonId = 2
                },
                new Student()
                {
                    Dob = DateTime.Now.AddYears(-20),
                    GivenName = "Marry",
                    Surname = "StudentThree",
                    Title = "Ms",
                    Gpa = 2.1F,
                    PersonId = 3
                }
            };

            return students;
        }

        private Teacher GetTestTeacher() {
            return new Teacher() {
                PersonId = 4,
                Dob = DateTime.Now.AddYears(-25),
                GivenName = "John",
                Surname = "Smith",
                Title = "Mr"
            };
        }

        #endregion
    }
}
