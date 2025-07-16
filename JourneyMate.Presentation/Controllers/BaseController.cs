namespace JourneyMate.Presentation.Controllers;

    public class BaseController : Controller
    {
        protected IActionResult HandleNullResult<T>(T result, string defaultError = "An error occurred.", string notFoundMessage = "Resource not found.") where T : class
        {
            if (result != null)
            {
                return View(result);
            }
            if (ModelState.ErrorCount > 0)
            {
                return View(result); // Return with validation errors if any
            }
            return string.IsNullOrEmpty(notFoundMessage) ? NotFound(notFoundMessage) : StatusCode(500, defaultError);
        }

        protected IActionResult HandleOperationResult(bool success, string defaultError = "An error occurred.")
        {
            if (success)
            {
                return RedirectToAction(nameof(Index));
            }
            return StatusCode(500, defaultError);
        }
    }

