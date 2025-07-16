using JourneyMate.Application.ServicesApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace journeyMate.Api.Controllers
{
    public class ApiEventsController : BaseController
    {
        private readonly IIpiEnventService _ipiEnventService;

        public ApiEventsController(IIpiEnventService ipiEnventService)
        {
            _ipiEnventService = ipiEnventService;
        }

        [HttpGet("events")]
        public async Task<IActionResult> GetEventsAsync()
        {
            var events = await _ipiEnventService.GetIpiEvenstAsync();
            if (events == null || !events.Any())
            {
                return NotFound("No events found.");
            }
            return Ok(events);
        }
        [HttpGet("events/{id}")]
        public async Task<IActionResult> GetEventByIdAsync(int id)
        {
            var eventItem = await _ipiEnventService.GetIpiEventByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound($"Event with ID {id} not found.");
            }
            return Ok(eventItem);
        }
    }
}
