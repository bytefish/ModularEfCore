// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularEfCore.Context;
using ModularEfCore.Example.Database.Map;
using ModularEfCore.Example.Web.Controllers;
using ModularEfCore.Example.Web.Database;
using ModularEfCore.Factory;
using ModularEfCore.Map;
using ModularEfCore.Seed;


namespace ModularEfCore.Example.Web
{
    public class Startup
    {
        public IHostingEnvironment Environment { get; set; }

        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            Environment = env;

            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add a CORS Policy to allow "Everything":
            services.AddCors(o =>
            {
                o.AddPolicy("Everything", p =>
                {
                    p.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                });
            });

            // Register the Options:
            services.AddOptions();

            // Register Database Entity Maps:
            services.AddSingleton<IEntityTypeMap, CustomerMap>();

            // Register the Seed:
            services.AddSingleton<IDbContextSeed, DbContextSeed>();

            // Add the DbContextOptions:
            var dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), x => x.MigrationsAssembly("ModularEfCore.Migrations"))
                .Options;

            services.AddSingleton(dbContextOptions);

            // Finally register the DbContextOptions:
            services.AddSingleton<ApplicationDbContextOptions>();

            // This Factory is used to create the DbContext from the custom DbContextOptions:
            services.AddSingleton<IApplicationDbContextFactory, ApplicationDbContextFactory>();

            // Finally Add the Applications DbContext:
            services.AddDbContext<ApplicationDbContext>();
            
            services
                // Use MVC:
                .AddMvc()
                // Add Application Modules:
                .AddApplicationPart(typeof(CustomerController).Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(policyName: "Everything");
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
