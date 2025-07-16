using JourneyMate.Core.Models.Company_Aggregation;
using JourneyMate.Application.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace JourneyMate.Presentation.Controllers
{
    [Authorize(Roles = "Company")]
    public class ItineraryController : Controller
    {
        private readonly ITravelService _travelService;
        private readonly IitineraryService _itineraryService;

        public ItineraryController(ITravelService travelService, IitineraryService itineraryService)
        {
            _travelService = travelService;
            _itineraryService = itineraryService;
        }

        [HttpGet("Itinerary/CreateItinerary/{travelId}")]
        public async Task<IActionResult> CreateItinerary(int travelId)
        {
            try
            {
                var travel = await _travelService.GetTravelByIdAsync(travelId);
                if (travel == null)
                {
                    TempData["ErrorMessage"] = "Travel package not found.";
                    return NotFound();
                }

                // Set ViewBag data
                ViewBag.TravelId = travelId;
                ViewBag.TravelTitle = travel.Title;

                // Store in TempData for POST action
                TempData["TravelId"] = travelId;
                TempData["TravelTitle"] = travel.Title;

                // Initialize a new ItineraryDayVm with default values
                var model = new ItineraryDayVm
                {
                    TravelId = travelId,
                    // Don't set Id - it will be auto-generated
                    DayNumber = 1, // Default to day 1
                    Title = "Day 1: ",
                    Activities = new List<string>()
                };

                return View(model);
            }
            catch (Exception ex)
            {
                // Log the exception
                TempData["ErrorMessage"] = "An error occurred while loading the page. Please try again.";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateItinerary(ItineraryDayVm itinerary)
        {
            try
            {
                // Ensure ID is not set (auto-generated)
                itinerary.Id = 0;

                // Additional server-side validation
                if (itinerary.StartTime.HasValue && itinerary.EndTime.HasValue)
                {
                    if (itinerary.StartTime >= itinerary.EndTime)
                    {
                        ModelState.AddModelError("EndTime", "End time must be after start time.");
                    }
                }

                // Validate day number
                if (itinerary.DayNumber < 1 || itinerary.DayNumber > 365)
                {
                    ModelState.AddModelError("DayNumber", "Day number must be between 1 and 365.");
                }

                // Check if TravelId exists
                var travel = await _travelService.GetTravelByIdAsync(itinerary.TravelId);
                if (travel == null)
                {
                    ModelState.AddModelError("TravelId", "Invalid travel package.");
                }

                // Optional: Check for duplicate day numbers for the same travel
                // You might want to implement this check in your service layer

                if (ModelState.IsValid)
                {
                    await _itineraryService.CreateItineraryAsync(itinerary);

                    TempData["SuccessMessage"] = $"Itinerary day '{itinerary.Title}' has been created successfully!";

                    // Redirect to travel details or itinerary list
                    return RedirectToAction("List", "Itinerary", new { id = itinerary.TravelId });
                }
            }
            catch (Exception ex)
            {
                // Log the exception here
                ModelState.AddModelError("", "An error occurred while creating the itinerary. Please try again.");
            }

            // If we got this far, something failed, redisplay form
            // Restore ViewBag data for the form
            ViewBag.TravelId = itinerary.TravelId;

            // Try to get travel title again or use TempData
            try
            {
                if (TempData.ContainsKey("TravelTitle"))
                {
                    ViewBag.TravelTitle = TempData["TravelTitle"];
                    TempData.Keep("TravelTitle"); // Keep for next request
                }
                else
                {
                    var travel = await _travelService.GetTravelByIdAsync(itinerary.TravelId);
                    ViewBag.TravelTitle = travel?.Title ?? "Unknown Travel";
                }
            }
            catch
            {
                ViewBag.TravelTitle = "Unknown Travel";
            }

            return View(itinerary);
        }

        // Optional: Add an action to get existing itineraries for a travel
        [HttpGet("Itinerary/List/{travelId}")]
        public async Task<IActionResult> ListItineraries(int travelId)
        {
            try
            {
                var travel = await _travelService.GetTravelByIdAsync(travelId);
                if (travel == null)
                {
                    return NotFound();
                }

                // Assuming you have a method to get itineraries by travel ID
                var itineraries = await _itineraryService.GetItinerariesByTravelIdAsync(travelId);

                ViewBag.TravelId = travelId;
                ViewBag.TravelTitle = travel.Title;

                // Return view with itineraries list
                return View(itineraries);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading itineraries.";
                return RedirectToAction("Details", "Travel", new { id = travelId });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteItinerary(int id, int travelId)
        {
            try
            {
                // Optional: Verify the itinerary belongs to the specified travel
                var itinerary = await _itineraryService.GetItineraryByIdAsync(id);
                if (itinerary == null)
                {
                    TempData["ErrorMessage"] = "Itinerary not found.";
                    return RedirectToAction("ListItineraries", new { travelId = travelId });
                }

                if (itinerary.TravelId != travelId)
                {
                    TempData["ErrorMessage"] = "Unauthorized action.";
                    return RedirectToAction("ListItineraries", new { travelId = travelId });
                }

                bool deleted = await _itineraryService.DeleteItineraryAsync(id);

                if (deleted)
                {
                    TempData["SuccessMessage"] = $"Itinerary '{itinerary.Title}' has been deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete the itinerary. Please try again.";
                }
            }
            catch (Exception ex)
            {
                // Log the exception here
                TempData["ErrorMessage"] = "An error occurred while deleting the itinerary. Please try again.";
            }

            return RedirectToAction("ListItineraries", new { travelId = travelId });
        }

        // Alternative: If you prefer using GET with confirmation (less secure but simpler)
        [HttpPost]
        public async Task<IActionResult> DeleteItineraryConfirm(int id, int travelId)
        {
            try
            {
                var itinerary = await _itineraryService.GetItineraryByIdAsync(id);
                if (itinerary == null || itinerary.TravelId != travelId)
                {
                    TempData["ErrorMessage"] = "Itinerary not found.";
                    return RedirectToAction("ListItineraries", new { travelId = travelId });
                }

                bool deleted = await _itineraryService.DeleteItineraryAsync(id);

                if (deleted)
                {
                    TempData["SuccessMessage"] = $"Itinerary '{itinerary.Title}' has been deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete the itinerary. Please try again.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the itinerary. Please try again.";
            }

            return RedirectToAction("ListItineraries", new { travelId = travelId });
        }

        // Add these methods to your existing ItineraryController class

        [HttpGet("Itinerary/EditItinerary/{id}")]
        public async Task<IActionResult> EditItinerary(int id)
        {
            try
            {
                var itinerary = await _itineraryService.GetItineraryByIdAsync(id);
                if (itinerary == null)
                {
                    TempData["ErrorMessage"] = "Itinerary not found.";
                    return NotFound();
                }

                // Get travel information
                var travel = await _travelService.GetTravelByIdAsync(itinerary.TravelId);
                if (travel == null)
                {
                    TempData["ErrorMessage"] = "Travel package not found.";
                    return NotFound();
                }

                // Set ViewBag data
                ViewBag.TravelId = itinerary.TravelId;
                ViewBag.TravelTitle = travel.Title;

                // Store in TempData for POST action
                TempData["TravelId"] = itinerary.TravelId;
                TempData["TravelTitle"] = travel.Title;

                return View(itinerary);
            }
            catch (Exception ex)
            {
                // Log the exception
                TempData["ErrorMessage"] = "An error occurred while loading the itinerary. Please try again.";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditItinerary(ItineraryDayVm itinerary)
        {
            try
            {
                // Additional server-side validation
                if (itinerary.StartTime.HasValue && itinerary.EndTime.HasValue)
                {
                    if (itinerary.StartTime >= itinerary.EndTime)
                    {
                        ModelState.AddModelError("EndTime", "End time must be after start time.");
                    }
                }

                // Validate day number
                if (itinerary.DayNumber < 1 || itinerary.DayNumber > 365)
                {
                    ModelState.AddModelError("DayNumber", "Day number must be between 1 and 365.");
                }

                // Check if TravelId exists
                var travel = await _travelService.GetTravelByIdAsync(itinerary.TravelId);
                if (travel == null)
                {
                    ModelState.AddModelError("TravelId", "Invalid travel package.");
                }

                // Optional: Check for duplicate day numbers for the same travel (excluding current itinerary)
                // You might want to implement this check in your service layer

                if (ModelState.IsValid)
                {
                    bool updated = await _itineraryService.UpdateItineraryAsync(itinerary);

                    if (updated)
                    {
                        TempData["SuccessMessage"] = $"Itinerary day '{itinerary.Title}' has been updated successfully!";
                        return RedirectToAction("ListItineraries", new { travelId = itinerary.TravelId });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to update the itinerary. Please try again.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception here
                ModelState.AddModelError("", "An error occurred while updating the itinerary. Please try again.");
            }

            // If we got this far, something failed, redisplay form
            // Restore ViewBag data for the form
            ViewBag.TravelId = itinerary.TravelId;

            // Try to get travel title again or use TempData
            try
            {
                if (TempData.ContainsKey("TravelTitle"))
                {
                    ViewBag.TravelTitle = TempData["TravelTitle"];
                    TempData.Keep("TravelTitle"); // Keep for next request
                }
                else
                {
                    var travel = await _travelService.GetTravelByIdAsync(itinerary.TravelId);
                    ViewBag.TravelTitle = travel?.Title ?? "Unknown Travel";
                }
            }
            catch
            {
                ViewBag.TravelTitle = "Unknown Travel";
            }

            return View(itinerary);
        }
    }
}