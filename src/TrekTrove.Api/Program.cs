using System.Diagnostics.CodeAnalysis;

namespace TrekTrove.Api
{
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            return Host
                .CreateDefaultBuilder(args)
                .ConfigureLogging(logginBuilder =>
                {
                    logginBuilder.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((hostingContext, builder) => {
                        if (!string.IsNullOrEmpty(environment) && environment.ToLower() == "local")
                        {
                            builder.AddUserSecrets<Startup>();
                        }
                    });
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
