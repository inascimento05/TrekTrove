using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics.CodeAnalysis;

namespace TrekTrove.Api.Bootstrap.Filters
{
    [ExcludeFromCodeCoverage]
    public class DeserializeHeadersFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await next();
        }
    }
}
