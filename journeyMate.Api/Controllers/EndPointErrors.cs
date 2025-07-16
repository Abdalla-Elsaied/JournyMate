using journeyMate.Api.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace journeyMate.Api.Controllers
{

    [ApiExplorerSettings(IgnoreApi = true)]
    public class EndPointErrors : BaseController
    {
        public ActionResult Errors(int code)
        {
            return NotFound(new ErrorResponse(code, "not found the endpoint"));
        }
    }
}
