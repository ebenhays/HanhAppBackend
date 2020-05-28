using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hahn.ApplicatonProcess.May2020.Data;
using Hahn.ApplicatonProcess.May2020.Data.Repository;
using Hahn.ApplicatonProcess.May2020.Domain.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using FluentValidation.AspNetCore;
using Hahn.ApplicatonProcess.May2020.Domain.BusinessLayer.Models;
using Hahn.ApplicatonProcess.May2020.Domain.BusinessLayer.Filters;
using FluentValidation;
using System.Reflection;
using System.IO;
using Serilog;
using Microsoft.Net.Http.Headers;

namespace Hahn.ApplicatonProcess.Application
{
    public class Startup
    {
        
        public Startup(IConfiguration configuration)
        {

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => {
                options.AddPolicy(name: "MyPolicy",
                              builder =>
                              {
                                  builder.WithOrigins("http://localhost:8080")
                                  .WithHeaders(HeaderNames.ContentType, "x-custom-header")
                                  .WithMethods("PUT", "DELETE", "GET","POST");

                              });
            });

            services.AddControllers().AddNewtonsoftJson();
            services.AddDbContext<ApplicantDBContext>(options => options.UseInMemoryDatabase(databaseName: "ApplicantsDB"));
            services.AddSingleton<IApplicant, ApplicantRepository>();
			services
				.AddMvc(options =>
				{
					options.Filters.Add<ValidationFilter>();
				})
				.AddFluentValidation(applicant => applicant.RegisterValidatorsFromAssemblyContaining<ApplicantValidator>());

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title="Hans Applicant API",
                    Description="Demo API of Applicants for Hans Application process",
                    Version="v1"
                });

                //show summary documentation on swagger endpoints
                var fileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var filePath = Path.Combine(AppContext.BaseDirectory, fileName);
                options.IncludeXmlComments(filePath);

            });
            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo API for Hans Application process");
                options.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseSerilogRequestLogging();

            app.UseRouting();
            app.UseCors();

            app.UseAuthorization();

          
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

           
        }
    }
}
