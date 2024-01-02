using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrekTrove.Api.Modules.CustomerModule.Data.Context;
using System.Data;

namespace TrekTrove.Api.Modules.CustomerModule.Infrastructure.Bootstrapers
{
    public static class ContextBootstrap
    {
        public static IServiceCollection ConfigureContextDb(
            this IServiceCollection services,
            IConfiguration configuration)
        {            
            var connectionString = configuration.GetConnectionString("Customer");
            services.AddTransient<IDbConnection>(b =>
            {
                return new SqlConnection(connectionString);
            });

            services.AddDbContextPool<CustomerDbContext>(options =>
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
            using var context = scope.ServiceProvider.GetRequiredService<CustomerDbContext>();

            context.Database.Migrate();
        }
    }
}
