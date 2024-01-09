using System.ComponentModel;

namespace TrekTrove.Api.Modules.SharedModule.Application.Notifications
{
    public enum ErrorCode
    {
        [Description("Not is possible to {0} the {1}")]
        BadRequest = 400,
        [Description("Need authorization to {0} the {1}")]
        Unauthorized = 401,
        [Description("Is forbidden to {0} the {1}")]
        Forbidden = 403,
        [Description("Could not be found the {1} to {0}")]
        NotFound = 404,
        [Description("Not allowed {0} the {1}")]
        MethodNotAllowed = 405,
        [Description("Unable to process the {1} to {0}")]
        UnprocessableEntity = 422,
        [Description("An unexpected condition prevents the {1} to be {0}")]
        InternalServerError = 500,
        [Description("Is not possible to {0} the {1} by an invalid response from the upstream server")]
        BadGateway = 502,
        [Description("The server is not ready to {0} the {1}")]
        ServiceUnavailable = 503,
    }
}
