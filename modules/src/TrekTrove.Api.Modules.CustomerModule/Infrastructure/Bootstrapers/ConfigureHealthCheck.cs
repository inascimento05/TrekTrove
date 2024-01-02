using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.Mime;
using System.Text.Json;

namespace TrekTrove.Api.Modules.CustomerModule.Infrastructure.Bootstrapers
{
    public static class HealthCheckBootstrap
    {
        public static IServiceCollection ConfigureHealthCheck(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddSqlServer(
                    configuration.GetConnectionString("Customer"),
                    name: "CustomerModule"
                );

            return services;
        }

        public static IApplicationBuilder ConfigureHealthCheck(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/Customer/ping");

            app.UseHealthChecks(
                "/Customer/pong",
                new HealthCheckOptions() { ResponseWriter = WritePongResultAsync });

            return app;
        }

        private static async Task WritePongResultAsync(
            HttpContext context,
            HealthReport report)
        {
            var pongResult = report.Entries
                  .OrderBy(x => x.Value.Status)
                  .Select(x => new
                  {
                      Resource = x.Key,
                      Status = x.Value.Status.ToString(),
                      x.Value.Tags
                  });

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
