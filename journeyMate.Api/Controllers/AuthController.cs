using Google.Apis.Auth;
using JourneyMate.Application.DTOs;
using JourneyMate.Application.ServicesApi.Interfaces;
using JourneyMate.Application.ViewModel.CreationVM;
using Microsoft.AspNetCore.Mvc;
using static Org.BouncyCastle.Math.EC.ECCurve;
using System.IdentityModel.Tokens.Jwt;

namespace journeyMate.Api.Controllers
{
   
    
    public class AuthController : BaseController
    {
        private readonly IUserAccountService _userAccountService;
        

        public AuthController(IUserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateAsync([FromBody] UserSignUpVm userCreate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AuthModel? result = await _userAccountService.CreateUser(userCreate);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);


            return Ok(result);

        }

        [HttpPost("login")]
        public async Task<IActionResult> LogInAsync([FromBody] UserLogInVm userLoger)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AuthModel? result = await _userAccountService.UserLoger(userLoger);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);
           
            return Ok(result);

        }

        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userAccountService.SendResetPasswordUrlAsync(model.Email ,model.Url );

            if (result is false)
                return NotFound("email is not found");

            return Ok(result);
        }

        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordDto model, string token)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userAccountService.ResetPasswordAsync(model);

            return Ok(result);
        }

        [HttpPost("googlelogin")]
        public async Task<IActionResult> GoogleLogin([FromBody] string idToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(idToken))
                    return BadRequest("Token is required.");

                var result = await _userAccountService.GoogleLogin(idToken);


                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


    }
}
