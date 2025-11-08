using Fin_X.Application.Services;
using Fin_X.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personal.Common.Domain;
using Personal.Common.Domain.Interfaces.Services;
using Porter.Api.Controllers;
using static Personal.Common.Handlers.Authentication.BasicAuthenticationHandler;

namespace Fin_X.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : CustomBaseController
    {
        private readonly ITokenService _tokenService;

        public LoginController(ILogger<PatientController> logger, ITokenService tokenService) : base(logger)
        {
            _tokenService = tokenService;
        }

     
        [HttpPost(Name = "Login")]
        [Authorize(AuthenticationSchemes = BasicAuthenticationOptions.DefaultScheme)]
        public IActionResult Login()
        {
            var username = User.Identity?.Name;
            var result = _tokenService.GenerateToken(username);


            if (result.IsFailure)
            {
                return base.CreateResponseFromResult(result);

            }
            return StatusCode(StatusCodes.Status201Created, result);
        }


    }
}
