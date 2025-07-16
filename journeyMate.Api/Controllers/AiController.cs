using System.Security.Claims;
using JourneyMate.Application.DTOs.Recommendation;
using JourneyMate.Application.ServicesApi.Interfaces;
using JourneyMate.Application.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace journeyMate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AiController : ControllerBase
    {
        private IRecommendationServices _recommendationServices;

        public AiController(IRecommendationServices recommendationServices)
        {
            _recommendationServices = recommendationServices;
        }
        [HttpPost("userinteractions")]

        public async Task<IActionResult> AddUserInteractions([FromBody] UserInteractionDto interactions)
        {
            //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //interactions.Id = userId;
            if (interactions == null )
            {
                return BadRequest("Interaction list is empty.");
            }

            var success = await _recommendationServices.UpdateInteractionsAsync(interactions);

            if (success)
            {
                return Ok("User interactions added successfully.");
            }

            return StatusCode(500, "Failed to add user interactions.");
        }
        
        [HttpGet("recommend-travels/{userId}")]
        public async Task<IActionResult> GetUserRecommendations(string userId,[FromQuery] int? numRecommendations = 5, [FromQuery] int? numHighestInteractions = 2)
        {
            

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found in token.");

            var recommendedTravels = await _recommendationServices.GetRecommendedTravelsAsync(
                userId, numRecommendations, numHighestInteractions);

            if (recommendedTravels == null || !recommendedTravels.Any())
                return NotFound("No recommendations found.");

            return Ok(recommendedTravels);
        }
        
        [HttpGet("recommend-events/{userId}")]
        public async Task<IActionResult> GetRecommendedEvents(string userId, [FromQuery] int? numRecommendations = null, [FromQuery] int? numHighestInteractions = null)
        {
            var recommendedEvents = await _recommendationServices.GetRecommendedEventsAsync(userId, numRecommendations, numHighestInteractions);

            if (recommendedEvents == null || !recommendedEvents.Any())
                return NotFound("No recommended events found.");

            return Ok(recommendedEvents);
        }
    }
}
