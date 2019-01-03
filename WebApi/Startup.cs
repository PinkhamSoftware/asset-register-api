using Infrastructure.Api.Middleware;
using Infrastructure.Documentation;
using Main;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HomesEngland.Gateway.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using WebApiContrib.Core.Formatter.Csv;

namespace WebApi
{
    public class Startup
    {
        private readonly string _apiName;

        public Startup(IConfiguration configuration)
        {
            _apiName = "Asset Register Api";
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<IApiVersionDescriptionProvider, DefaultApiVersionDescriptionProvider>();

            var csvOptions = new CsvFormatterOptions
            {
                UseSingleLineHeaderInCsv = true,
                CsvDelimiter = ",",
                IncludeExcelDelimiterHeader = true
            };

            services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = true;
                //options.InputFormatters.Add(new CsvInputFormatter(csvFormatterOptions));
                options.OutputFormatters.Add(new CsvOutputFormatter(csvOptions));
                options.FormatterMappings.SetMediaTypeMappingForFormat("csv", MediaTypeHeaderValue.Parse("text/csv"));
            }).AddCsvSerializerFormatters(csvOptions);

            var assetRegister = new AssetRegister();
            assetRegister.ExportDependencies((type, provider) => services.AddTransient(type, _ => provider()));

            assetRegister.ExportTypeDependencies((type, provider) => services.AddTransient(type, provider));

            services.ConfigureDocumentation(_apiName);

            services
                .AddEntityFrameworkNpgsql()
                .AddDbContext<AssetRegisterContext>();
            
            var serviceProvider = services.BuildServiceProvider();
            var assetRegisterContext = serviceProvider.GetService<AssetRegisterContext>();
            assetRegisterContext.Database.Migrate();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IConfiguration configuration)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.ConfigureSwaggerUi(_apiName);
            app.UseCors(builder =>
                builder.WithOrigins(configuration["CorsOrigins"].Split(";"))
                       .AllowAnyHeader()
                       .AllowAnyMethod()
            );

            app.UseMiddleware<CustomExceptionHandlerMiddleware>();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
