using EVMDealerSystem.BusinessLogic.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EVMDealerSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected ActionResult<Result<T>> HandleResult<T>(Result<T> result)
        {
            return result.ResultStatus switch
            {
                ResultStatus.Success => Ok(result),
                ResultStatus.NotFound => NotFound(result),
                ResultStatus.Invalid => BadRequest(result),
                ResultStatus.Conflict => Conflict(result),
                ResultStatus.Unauthorized => Unauthorized(result),
                ResultStatus.Forbidden => Forbid(), 
                ResultStatus.InternalServerError => StatusCode(500, result),
                _ => StatusCode(500, Result<T>.InternalServerError("Unknown error."))
            };
        }
    }
}
