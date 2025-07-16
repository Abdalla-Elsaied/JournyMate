using JourneyMate.Application.DTOs;
using JourneyMate.Application.ServicesApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace journeyMate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourismCompanyRequestApiController(ITourismCompanyRequestService tourismCompanyRequestService) : ControllerBase
    {
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<TourismCompanyRequestDto>> CreateTourismCompanyRequest([FromForm] TourismCompanyRequestDto tourismCompanyRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(tourismCompanyRequestDto == null)
            {
                return BadRequest("Tourism company request cannot be null.");
            }
            var result = await tourismCompanyRequestService.CreateTourismCompanyRequestAsync(tourismCompanyRequestDto);
            if (!result)
            {
                return BadRequest("Failed to create tourism company request.");
            }
            return Ok(result);
        }
    }
}
