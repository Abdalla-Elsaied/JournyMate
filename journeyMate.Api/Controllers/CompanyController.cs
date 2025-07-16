
using journeyMate.Api.Controllers;
using journeyMate.Api.Errors;
using JourneyMate.Application.Common;
using JourneyMate.Application.DTOs;
using JourneyMate.Application.ServicesApi.Interfaces;
using JourneyMate.Core.Models;
using JourneyMate.Core.Models.Company_Aggregation;
using JourneyMate.Infrastracture.Specefications.CompanySepcifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JournyMate.Api.Controllers
{

    public class CompanyController : BaseController
    {
        private readonly ICompanyServices _companyServices;
        private readonly IRateServices _rateServices;
        private readonly UserManager<AppUser> _userManager;

        public CompanyController(ICompanyServices companyServices, IRateServices rateServices,UserManager<AppUser> userManager)
        {
            _companyServices = companyServices;
            _rateServices = rateServices;
            _userManager = userManager;
        }
        [HttpPost("rate")]
        public async Task<IActionResult> RateCompany([FromBody] CompanyRatingDto ratingDto)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(userEmail);
            ratingDto.UserId = user.Id;
            if (ratingDto.Rating is < 1 or > 5)
                return BadRequest("Rating must be between 1 and 5.");

            var company = await _companyServices.GetCompanyById(ratingDto.CompanyId);
            if (company == null)
                return NotFound("Company not found.");

            var AddedorUpdatedRate = await _rateServices.AddOrUpdateUserRating(ratingDto);
            var average = await _rateServices.GetAverageRatingForCompany(company.Id);
            company.Rating = average;
            var result = await _companyServices.UpdateRate(company);

            return Ok(new
            {
                CompanyData = result,
                average,
                yourRating = ratingDto.Rating,
                message = "Thanks for rating!"
            });
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PagedResult<CompanyDetailsDto>>> GetAllCompies([FromQuery] CompanySpecParams companySpec)
        {
            var Companies = await _companyServices.GetAllCompaniessBySpec(companySpec);
            if (Companies == null)
                return NotFound(new ErrorResponse(404));
            var datacount = await _companyServices.GetCountAsync(companySpec);
            PagedResult<CompanyDetailsDto> DataPagination = new PagedResult<CompanyDetailsDto>(Companies, datacount, companySpec.PageIndex, companySpec.PageSize);

            return Ok(DataPagination);

        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CompanyDetailsDto>> GetCompanyById([FromRoute] int id)
        {
            var company = await _companyServices.GetCompanyById(id);
            if (company == null)
                return NotFound(new ErrorResponse(404));

            return Ok(company);
        }
        [HttpGet("image/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<string>>> GetImagesForCompany([FromRoute] int id)
        {
            var images = await _companyServices.GetCompanyImages(id);

            return images == null ?  NotFound(new ErrorResponse(404)) : Ok(images);
        }
    }
}