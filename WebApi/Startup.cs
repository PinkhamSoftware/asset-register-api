using System.Text;
using HomesEngland.BackgroundProcessing;
using Infrastructure.Documentation;
using Main;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HomesEngland.Gateway.Migrations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                var secret = Configuration["HmacSecret"];
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = key,
                    RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateAudience = false
                };
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<IApiVersionDescriptionProvider, DefaultApiVersionDescriptionProvider>();

            services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = true;
                options.OutputFormatters.Add(new CsvOutputFormatter(GetCsvOptions()));
                options.FormatterMappings.SetMediaTypeMappingForFormat("csv", MediaTypeHeaderValue.Parse("text/csv"));
                
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddCsvSerializerFormatters(GetCsvOptions());

            var assetRegister = new AssetRegister();
            assetRegister.ExportDependencies((type, provider) => services.AddTransient(type, _ => provider()));

            assetRegister.ExportTypeDependencies((type, provider) => services.AddTransient(type, provider));

            assetRegister.ExportSingletonDependencies((type, provider) => services.AddSingleton(type, _ => provider()));

            assetRegister.ExportSingletonTypeDependencies((type, provider) => services.AddSingleton(type, provider));

            services.ConfigureDocumentation(_apiName);


            services
                .AddEntityFrameworkNpgsql()
                .AddDbContext<AssetRegisterContext>();

            AssetRegisterContext assetRegisterContext =
                services.BuildServiceProvider().GetService<AssetRegisterContext>();
            assetRegisterContext.Database.Migrate();

            services.AddHostedService<BackgroundProcessor>();
        }

        private static CsvFormatterOptions GetCsvOptions()
        {
            return new CsvFormatterOptions
            {
                UseSingleLineHeaderInCsv = true,
                CsvDelimiter = ",",
                IncludeExcelDelimiterHeader = true
            };
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IConfiguration configuration)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.ConfigureSwaggerUi(_apiName);
            app.UseCors(builder =>
                builder.WithOrigins(configuration["CorsOrigins"].Split(";"))
                    .AllowAnyHeader()
                    .AllowAnyMethod()
            );

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
