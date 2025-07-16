
using journeyMate.Api.Controllers;
using journeyMate.Api.Errors;
using JourneyMate.Application.Common;
using JourneyMate.Application.DTOs;
using JourneyMate.Application.ServicesApi.Interfaces;
using JourneyMate.Infrastracture.Specefications.TravelSpecifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace JournyMate.Api.Controllers
{
    
    public class TravelController(ITravelServices travelServices) : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<PagedResult<TravelListDto>>> GetTravels([FromQuery] TravelSpecPrams travelPrams)
        {
            var travels = await travelServices.GetAllTravles(travelPrams);
            if (travels is null)
            {
                return NotFound();   
            }
            var spec = new TravelSpecifications(travelPrams);
            var count  = await travelServices.GetCountAsync(travelPrams,spec); 
            var paging = new PagedResult<TravelDetailsDto>(travels, count, travelPrams.PageIndex, travelPrams.PageSize);
            return Ok(paging);      
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> getTravelById([FromRoute]int id)
        {
          
            var travles = await travelServices.GetTravelById(id);
            if (travles is null)
                return NotFound("No travles for Compnay");
            return Ok(travles);     
        }
        [HttpGet("discounted")]
        public async Task<ActionResult<PagedResult<TravelListDto>>> GetDiscountedTravels([FromQuery] TravelSpecPrams travelPrams)
        {
            var travels = await travelServices.GetDiscountedTravles(travelPrams);
            
            if (travels is null)
            {
                return NotFound();
            }
            var spec = new DiscountedTravelSpecs(travelPrams);      
            var count = await travelServices.GetCountAsync(travelPrams, spec);
            var paging = new PagedResult<TravelDetailsDto>(travels, count, travelPrams.PageIndex, travelPrams.PageSize);
            return Ok(paging);     
        }

        [HttpGet("leaving-soon")]
        public async Task<ActionResult<PagedResult<TravelListDto>>> GetTravelsLeavingSoon([FromQuery] TravelSpecPrams travelPrams)
        {
            var travels = await travelServices.GetTravelsLeavingSoon(travelPrams);
            if (travels is null)
            {
                return NotFound();
            }
            var spec = new LeavingSoonTravelSpecs(travelPrams);     
            var count = await travelServices.GetCountAsync(travelPrams, spec);
            var paging = new PagedResult<TravelDetailsDto>(travels, count, travelPrams.PageIndex, travelPrams.PageSize);
            return Ok(paging);
        }
        [Authorize]
        [HttpPost("like/{travelId}")]     
        public async Task<IActionResult> LikeTravel(int travelId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);  //User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            var result = await travelServices.AddLikeToTravel(travelId, userId);
            if(!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);      

        }

        [Authorize]
        [HttpDelete("like/{TravelId}")]
        public async Task<IActionResult> RemoveLikeFromTravel(int TravelId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); //User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var result = await travelServices.RemoveLikeFromTravel(TravelId, userId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }
        [Authorize]
        [HttpGet("Favtravels")]
        public async Task<ActionResult<List<TravelListDto>?>> FavTravels()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); //User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var FavtravelsDto = await travelServices.GetFavTravelsByUserId(userId);
            if (FavtravelsDto == null)
                return NotFound(new ErrorResponse(404));

            return Ok(FavtravelsDto);
        }


        [HttpGet("newest-for-company/{companyId}")]
        public async Task<ActionResult<PagedResult<TravelDetailsDto>>> GetNewestTravelForCompany(int companyId, [FromQuery] TravelSpecPrams travelParams)
        {
            if (companyId <= 0)
            {
                return BadRequest("Invalid company ID.");
            }
            if (travelParams.PageIndex < 1 || travelParams.PageSize < 1)
            {
                return BadRequest("Invalid pagination parameters.");
            }

            var travels = await travelServices.GetNewestTravelsForCompany(travelParams,companyId);
              
            if (travels == null || !travels.Any())
            {
                return NoContent();
            }

            var spec = new NewestTravelSpecifications(travelParams, companyId);
        
            var count = await travelServices.GetCountAsync(travelParams,spec);
            var paging = new PagedResult<TravelDetailsDto>(travels, count, travelParams.PageIndex, travelParams.PageSize);
            return Ok(paging);
        }

        [HttpGet("newest")]
        public async Task<ActionResult<PagedResult<TravelDetailsDto>>> GetNewestTravels( [FromQuery] TravelSpecPrams travelParams)
        {
           
            if (travelParams.PageIndex < 1 || travelParams.PageSize < 1)
            {
                return BadRequest("Invalid pagination parameters.");
            }

            var travels = await travelServices.GetNewestTravels(travelParams);

            if (travels == null || !travels.Any())
            {
                return NotFound("No travels found for the specified company.");
            }

            var spec = new NewestTravelSpecifications(travelParams);


            var count = await travelServices.GetCountAsync(travelParams, spec);
            var paging = new PagedResult<TravelDetailsDto>(travels, count, travelParams.PageIndex, travelParams.PageSize);
            return Ok(paging);
        }

    }
}
