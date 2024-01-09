using Microsoft.AspNetCore.Mvc;

namespace TrekTrove.Api.Presenters
{
    public interface IPresenter
    {
        IActionResult GetResult<T>(DataResult<T> result);
    }
}
