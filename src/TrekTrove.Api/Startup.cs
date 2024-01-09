using System.Diagnostics.CodeAnalysis;
using TrekTrove.Api.Presenters;
using TrekTrove.Api.Bootstrap;
namespace TrekTrove.Api
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureApi(Configuration);

            services.AddTransient<IPresenter, Presenter>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.ConfigureApi(env);
        }
    }
}
