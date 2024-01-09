using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Diagnostics.CodeAnalysis;
using TrekTrove.Api.Bootstrap.Bootstrapers.Options;

namespace TrekTrove.Api.Bootstrap.Bootstrapers
{
    [ExcludeFromCodeCoverage]
    public static class SwaggerBootstrap
    {
        public static IServiceCollection ConfigureSwagger(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var section = configuration.GetSection(ApiOptions.AppSettingsSection) ?? throw new InvalidOperationException($"Configuration section '{ApiOptions.AppSettingsSection}' not found.");
            var swaggerOptions = section.Get<ApiOptions>() ?? throw new InvalidOperationException($"Configuration section '{section}' not found.");

            services.Configure<ApiOptions>(section);
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>();

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(swaggerOptions.DefaultMajorApiVersion, swaggerOptions.DefaultMinorApiVersion);
            });

            services.AddVersionedApiExplorer(p =>
            {
                p.DefaultApiVersion = new ApiVersion(swaggerOptions.DefaultMajorApiVersion, swaggerOptions.DefaultMinorApiVersion);
                p.GroupNameFormat = "'v'VVV";
                p.SubstituteApiVersionInUrl = true;
            });

            services.AddSwaggerGen(options =>
            {
                options.DescribeAllParametersInCamelCase();
                options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });

                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                options.IgnoreObsoleteActions();
                options.IgnoreObsoleteProperties();
                options.CustomSchemaIds(x => x.Name);
            });

            return services;
        }

        public static IApplicationBuilder ConfigureSwagger(this IApplicationBuilder app)
        {
            var swaggerOptions = app.ApplicationServices.GetRequiredService<IOptions<ApiOptions>>();
            var apiVersionProvider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var description in apiVersionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        $"{description.GroupName.ToUpperInvariant()} - {swaggerOptions.Value.ApiTitle}"
                    );
                }

                options.DocExpansion(DocExpansion.List);
            });

            return app;
        }
    }
}
