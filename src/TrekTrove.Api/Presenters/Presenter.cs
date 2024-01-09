using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using TrekTrove.Api.Models;

namespace TrekTrove.Api.Presenters
{
    [ExcludeFromCodeCoverage]
    public class Presenter : IPresenter
    {
        public virtual IActionResult GetResult<T>(DataResult<T> result)
        {
            if (result.Invalid || result.Error != null)
                return CreateErrorResult(result);

            return new OkObjectResult(result.Data);
        }

        private static IActionResult CreateErrorResult(Result result)
        {
            var errorBody = ApiError.FromResult(result);
            return result.Error switch
            {
                ErrorCode.NotFound => new NotFoundObjectResult(errorBody),
                ErrorCode.UnprocessableEntity => new UnprocessableEntityObjectResult(errorBody),
                ErrorCode.Unauthorized => new UnauthorizedObjectResult(errorBody),
                _ => new BadRequestObjectResult(errorBody),
            };
        }
    }
}
