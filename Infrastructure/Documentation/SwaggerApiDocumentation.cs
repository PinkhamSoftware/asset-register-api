using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Infrastructure.Documentation
{
    public static class SwaggerApiDocumentation
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="apiName"></param>
        public static void ConfigureDocumentation(this IServiceCollection services, string apiName)
        {
            services.AddSwaggerGen(c =>
            {
                var version = $"v{1}";
                c.SwaggerDoc(version, new Info { Title = $"{apiName} {version}", Version = version });

                c.CustomSchemaIds(x => x.FullName);

                IncludeXmlCommentsIfPresent(c);
            });
        }

        private static void IncludeXmlCommentsIfPresent(SwaggerGenOptions c)
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
                c.IncludeXmlComments(xmlPath);
        }

        /// <summary>
        /// Pre-Requisite Controllers must [ApiVersion("x")] on them
        /// </summary>
        /// <param name="app"></param>
        /// <param name="apiName"></param>
        /// <returns></returns>
        public static List<ApiVersionDescription> ConfigureSwaggerUi(this IApplicationBuilder app, string apiName)
        {
            var api = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();
            var apiVersions = api.ApiVersionDescriptions.Select(s => s).ToList();
            app.UseSwaggerUI(c =>
            {
                //Create a swagger endpoint for each swagger version
                c.SwaggerEndpoint($"v1/swagger.json", $"{apiName} 1");
            });

            app.UseSwagger();

            return apiVersions;
        }
    }
}
