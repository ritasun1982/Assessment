using Assessment.API.Application.BussinessLogicLayer.Interfaces;
using Assessment.API.Application.BussinessLogicLayer.Services;
using Assessment.API.Application.Controllers;
using Assessment.API.Application.DataAccessLayer.Data;
using Assessment.API.Application.DataAccessLayer.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;

namespace Assessment.API.Application
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

            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), 
                                                                                b => b.MigrationsAssembly("Assessment.API.Application")));

            services.AddTransient<IClientService, ClientService>();
            services.AddTransient<IValidationService, ValidationService>();

            services.AddLogging(options =>
            {
                options.SetMinimumLevel(LogLevel.Trace);

                //Add hook to the Application Insights Provider
                options.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Trace);
                options.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Information);
                options.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Error);

                options.AddFilter(typeof(ClientController).FullName, LogLevel.Trace);

                //Pass in the InstrumentationKey provided under the appsettings
                options.AddApplicationInsights(Configuration["ApplicationInsights:InstrumentationKey"]);

               
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Assessment.API.Application", Version = "v1" });
            });



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(errorApp =>
                {
                   
                    errorApp.Run(async context =>
                    {
                        var pd = new ProblemDetails
                        {
                            Type = "https://Assessment.API.Application/errors/internal-server-error",
                            Title = "An unrecoverable error occurred",
                            Status = StatusCodes.Status500InternalServerError,
                            Detail = "Exceptions",
                        };
                        pd.Extensions.Add("RequestId", context.TraceIdentifier);
                        await context.Response.WriteAsJsonAsync(pd, pd.GetType(), null, contentType: "application/problem+json");
                    });
                });
            }

           

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Assessment.API.Application v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
