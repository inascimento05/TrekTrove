using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrekTrove.Api.Models;

namespace TrekTrove.Api.Controllers
{
    [ApiConventionType(typeof(DefaultApiConventions))]
    [ApiController]
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiError))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiError))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiError))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ApiError))]
    public class BaseController : ControllerBase
    {
        protected readonly IMediator _mediator;
        protected readonly int DEFAULT_PAGE_NUMBER = 1;
        protected readonly int DEFAULT_PAGE_SIZE = 10;

        public BaseController(IConfiguration config, IMediator mediator)
        {
            int.TryParse(config.GetSection("DefaultPageSize").Value, out DEFAULT_PAGE_SIZE);

            _mediator = mediator;
        }
    }
}
