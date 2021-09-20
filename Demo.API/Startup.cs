using Demo.API.Domain.Repository;
using Demo.API.Domain.Service;
using Demo.Common.Configuration;
using Demo.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;

namespace ATS.API
{
    public class Startup
    {
        #region Constants
        private const string ApplicationName = "Demo.API";

        private const string ProductionEnvironment = "Prod";

        private const string SwaggerTitle = "Demo";
        private const string SwaggerDescription = "Aplicação Demo";
        private const string SwaggerVersion = "v1.0";

        private string[] DocumentationFiles => new string[]
        {
        };
        #endregion

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _ = services.AddCors(options =>
                {
                    options.AddPolicy("Policy1",
                    builder => builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
                });

            _ = services.AddControllers();
            _ = services.AddResponseCompression();

            _ = services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(SwaggerVersion, new OpenApiInfo
                {
                    Title = SwaggerTitle,
                    Description = SwaggerDescription,
                    Version = SwaggerVersion,
                    Contact = new OpenApiContact
                    {
                        Name = "Pedro",
                        Email = "puedroreis@gmail.com",
                    },
                });
                c.UseAllOfForInheritance();
                c.UseOneOfForPolymorphism();

                foreach (string xmlFile in DocumentationFiles)
                {
                    string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);
                }
            });

            _ = services.RegisterCommons(ApplicationName, Configuration);          
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            string targetValue;
            bool isProduction;

            string endpoint;
            string title;

            targetValue = Configuration.GetValue("TargetEnvironment")[0];
            isProduction = ProductionEnvironment.Equals(targetValue, StringComparison.OrdinalIgnoreCase);

            if (!isProduction)
            {
                endpoint = $"{SwaggerVersion}/swagger.json";
                title = $"{SwaggerTitle} {SwaggerVersion}";

                _ = app.UseDeveloperExceptionPage();

                _ = app.UseSwagger();
                _ = app.UseSwaggerUI(c => c.SwaggerEndpoint(endpoint, title));
            }

            _ = app.UseRouting();
            _ = app.UseCors("Policy1");
            _ = app.UseAuthorization();

            _ = app.UseStaticFiles();
            _ = app.UseResponseCompression();

            _ = app.UseAuthorization();
            _ = app.UseHttpsRedirection();
            _ = app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
