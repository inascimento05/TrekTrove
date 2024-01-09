using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics.CodeAnalysis;

namespace TrekTrove.Api.Bootstrap.Bootstrapers.Options
{
    [ExcludeFromCodeCoverage]
    public class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private IApiVersionDescriptionProvider _provider { get; }

        private IOptions<ApiOptions> _swaggerOptions { get; }

        public ConfigureSwaggerGenOptions(
            IApiVersionDescriptionProvider provider, 
            IOptions<ApiOptions> swaggerOptions)
        {
            _provider = provider;
            _swaggerOptions = swaggerOptions;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        private OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = _swaggerOptions.Value.ApiTitle,
                Version = description.ApiVersion.ToString(),
                Description = _swaggerOptions.Value.ApiDescription,
                Contact = new OpenApiContact()
                {
                    Name = _swaggerOptions.Value.ContactName,
                    Email = _swaggerOptions.Value.ContactEmail
                }
            };

            if (description.IsDeprecated)
            {
                info.Description += _swaggerOptions.Value.DeprecatedMessage;
            }

            return info;
        }
    }
}
