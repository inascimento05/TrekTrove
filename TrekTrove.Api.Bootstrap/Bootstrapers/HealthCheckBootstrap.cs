using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using System.Text.Json;

namespace TrekTrove.Api.Bootstrap.Bootstrapers
{
    [ExcludeFromCodeCoverage]
    public static class HealthCheckBootstrap
    {
        private const string HEALTHCHECK_PING_PATHSTRING = $"/ping";
        private const string HEALTHCHECK_PONG_PATHSTRING = $"/pong";
        public static IServiceCollection ConfigureHealthCheck(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            services.AddHealthChecks()
                .AddSqlServer(
                    connectionString,
                    name: "Default"
                );

            return services;
        }

        public static IApplicationBuilder ConfigureHealthCheck(this IApplicationBuilder app)
        {
            app.UseHealthChecks(HEALTHCHECK_PING_PATHSTRING);

            app.UseHealthChecks(
                HEALTHCHECK_PONG_PATHSTRING,
                new HealthCheckOptions() { ResponseWriter = WritePongResultAsync });

            return app;
        }

        private static async Task WritePongResultAsync(
            HttpContext context,
            HealthReport report)
        {
            var pongResult = "pong";

            context.Response.ContentType = MediaTypeNames.Application.Json;

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(
                    pongResult,
                    options: new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    }
                )
            );
        }
    }
}
