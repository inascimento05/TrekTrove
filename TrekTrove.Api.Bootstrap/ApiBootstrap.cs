using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;
using TrekTrove.Api.Bootstrap.Bootstrapers;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace TrekTrove.Api.Bootstrap
{
    [ExcludeFromCodeCoverage]
    public static class ApiBootstrap
    {
        public static IServiceCollection ConfigureApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers()
                    .AddJsonOptions(_ =>
                    {
                        _.JsonSerializerOptions
                            .Converters
                            .Add(new JsonStringEnumConverter());

                        _.JsonSerializerOptions
                            .DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    });
            services.AddHttpContextAccessor();
            services.ConfigureRoutes();

            services.AddAuthorizationAuthentication(configuration);
            services.ConfigureSwagger(configuration);


            services.ConfigureHealthCheck(configuration);
            services.ConfigureClientsServices(configuration);

            services.ConfigureModules(configuration);

            services.ConfigureFilters();

            services.ConfigureMVC();

            services
                .AddCors(options =>
                    options.AddDefaultPolicy(builder =>
                        builder.SetIsOriginAllowed(t => true)
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                    )
                );

            var featureFlags = configuration.GetSection("FeatureFlags");

            services.AddFeatureManagement(featureFlags);

            return services;
        }

        public static IApplicationBuilder ConfigureApi(
            this IApplicationBuilder app,
            IWebHostEnvironment env)
        {
            if (!env.IsProduction() && !env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
                app.ConfigureSwagger();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.ConfigureHealthCheck();

            app.ConfigureModules(env);

            return app;
        }
    }
}
