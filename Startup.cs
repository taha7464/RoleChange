using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RoleChange.Hubs;
using RoleChange.Services;

namespace RoleChange
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<JsonFileUserService>();
            services.AddControllers();
            services.AddSpaStaticFiles(configure =>
            {
                configure.RootPath = "client/build";
            });
            services.AddSignalR();
            services.AddCors(options =>
                options.AddPolicy("CorsPolicy",
                    builder =>
                        builder.AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithOrigins("http://localhost:5001")
                        .AllowCredentials()));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<MyHub>("/roleChangeHub");
                endpoints.MapControllers();
            });
            app.UseSpa(spa =>
           {
               spa.Options.SourcePath = "clientapp";
               if (env.IsDevelopment())
               {
                   spa.UseReactDevelopmentServer(npmScript: "start");
               }
           });
           app.UseCors("CorsPolicy");
        }
    }
}
