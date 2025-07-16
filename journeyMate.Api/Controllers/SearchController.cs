using JourneyMate.Application.ServicesApi.Interfaces;
using JourneyMate.Infrastracture.Specefications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace journeyMate.Api.Controllers
{

    public class SearchController(ISearchServices _searchServices) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Search(string? type,[FromQuery]BaseSpecPrams specPrams)
        {
            var result = await _searchServices.SearchAsync(type, specPrams.Search,specPrams.PageIndex,specPrams.PageSize);
            if (result is null)
                return NotFound("no result back");
            return Ok(result);

        }
    }
}
