using journeyMate.Api.Errors;
using JourneyMate.Application.ServicesApi.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace journeyMate.Api.Controllers
{
   
    public class BuggyController : BaseController
    {
        private readonly ICompanyServices _companyServices;
        public BuggyController(ICompanyServices companyServices)
        {
            _companyServices = companyServices;
        }
        [HttpGet("notfound")]
        public async Task<IActionResult> GetNotFound()
        {
            var company = await _companyServices.GetCompanyById(100);
            if (company is null) return NotFound(new ErrorResponse(404));
            return Ok(company);
        }
        [HttpGet("badrequest")]
        public IActionResult GetBadRequest()
        {
            return BadRequest(new ErrorResponse(400));
        }

        [HttpGet("servererror")]  //null refrance
        public IActionResult GetServerError()
        {
            var company = _companyServices.GetCompanyById(100);
            if (company is null)
            {
                var productToreturn = company.ToString();
            }
            return Ok(company);
        }

        [HttpGet("badrequest/{id}")]   // api/buggy/badrequest/five
        public IActionResult GetBadRequest(int id)
        {
            return Ok();
        }
    }
}
