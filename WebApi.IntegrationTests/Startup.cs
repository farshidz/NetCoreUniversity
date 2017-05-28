using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCoreUniversity.Data;
using NetCoreUniversity.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreUniversity.WebApi.IntegrationTests {
    public class Startup {
        public Startup(IHostingEnvironment env) {
            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            // Add framework services.
            services.AddMvc().AddJsonOptions(
                options => {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                }
            );

            services.AddEntityFrameworkSqlServer();

            // Use factory, so that we get the same DB instance as 
            // the test method
            services.AddTransient<SchoolContext>(
                p => new DbContextFactory().GetContext()
            );

            // Note: This must not be singleton -- DbContext does not support
            // multi-threading
            services.AddTransient<IDataRepositories, DataRepositories>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
