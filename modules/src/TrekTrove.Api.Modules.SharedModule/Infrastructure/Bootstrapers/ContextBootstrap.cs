using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrekTrove.Api.Modules.SharedModule.Data.Context;
using System.Data;

namespace TrekTrove.Api.Modules.SharedModule.Infrastructure.Bootstrapers
{
    public static class ContextBootstrap
    {
        public static IServiceCollection ConfigureContextDb(
            this IServiceCollection services,
            IConfiguration configuration)
        {            
            var connectionString = configuration.GetConnectionString("Shared");
            services.AddTransient<IDbConnection>(b =>
            {
                return new SqlConnection(connectionString);
            });

            services.AddDbContextPool<SharedDbContext>(options =>
                options.UseSqlServer(
                    connectionString
                )
            );

            return services;
        }

        public static void MigrateDatabaseOnStartup(
            this IApplicationBuilder builder)
        {
            using var scope = builder.ApplicationServices.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<SharedDbContext>();

            context.Database.Migrate();
        }
    }
}
