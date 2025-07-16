using journeyMate.Api.Errors;
using JourneyMate.Application.DTOs;
using JourneyMate.Application.IServeces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace journeyMate.Api.Controllers
{

    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CateroryDto>>> GetCategory()
        {
            var categories =await _categoryService.GetCaterories();
            if (categories == null)
            {
                return NotFound(new ErrorResponse(404));
            }
            return Ok(categories);
        }
    }
}
