using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JourneyMate.Presentation.Controllers
{
    [Authorize(Roles = "Super,Admin")]
    public class EventsController : Controller
    {
        private readonly IEventItemServece _eventItemServece;
      

        public EventsController(IEventItemServece eventItemServece)
        {
            _eventItemServece = eventItemServece;
        }

        public async Task<ActionResult> Index(
            string searchTerm = "",
            string category = "",
            string sortBy = "date",
            string sortOrder = "asc",
            int page = 1,
            int pageSize = 9)
        {
            try
            {
                var result = await _eventItemServece.GetEventsAsync(
                    searchTerm, category, sortBy, sortOrder, page, pageSize);

                if (result == null)
                {
                    ViewBag.ErrorMessage = "Unable to load events at this time.";
                    return View(new EventsViewModel());
                }

                return View(result);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading events.";
                return View(new EventsViewModel());
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid event ID.";
                return RedirectToAction("Index");
            }

            var result = await _eventItemServece.DeleteEventAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Event deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete event.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Confirm(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid event ID.";
                return RedirectToAction("Index");
            }

            var result = await _eventItemServece.ConfirmedEventAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Event confirmed successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to confirm event.";
            }

            return RedirectToAction("Index");
        }

        // AJAX endpoints for better UX
        [HttpPost]
        public async Task<JsonResult> DeleteEvent(int id)
        {
            if (id <= 0)
            {
                return Json(new { success = false, message = "Invalid event ID." });
            }

            var result = await _eventItemServece.DeleteEventAsync(id);
            return Json(new
            {
                success = result,
                message = result ? "Event deleted successfully." : "Failed to delete event."
            });
        }

        [HttpPost]
        public async Task<JsonResult> ConfirmEvent(int id)
        {
            if (id <= 0)
            {
                return Json(new { success = false, message = "Invalid event ID." });
            }

            var result = await _eventItemServece.ConfirmedEventAsync(id);
            return Json(new
            {
                success = result,
                message = result ? "Event confirmed successfully." : "Failed to confirm event."
            });
        }

        public async Task<JsonResult> GetCategories()
        {
            var categories = await _eventItemServece.GetCategoriesAsync();
            return Json(categories);
        }
    }
}