using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics.CodeAnalysis;

namespace TrekTrove.Api.Bootstrap.Filters
{
    [ExcludeFromCodeCoverage]
    public class DefaultHeaderFilter : IOperationFilter
    {
        private const string TOKEN_HEADER = "x-token";
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters.Add(new OpenApiParameter { Name = TOKEN_HEADER, In = ParameterLocation.Header, Required = true });
        }
    }
}
