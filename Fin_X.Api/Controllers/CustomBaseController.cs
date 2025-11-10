using Microsoft.AspNetCore.Mvc;
using Personal.Common.Domain;
using Personal.Common.Dto;

namespace Porter.Api.Controllers
{
    public abstract class CustomBaseController: ControllerBase
    {

        protected readonly ILogger<CustomBaseController> _logger;
        

        protected CustomBaseController(ILogger<CustomBaseController> logger)

        {
            _logger = logger;
        }

        protected IActionResult CreateResponseFromResult(Result result)
        {
            ErrorResponseDto error = new ErrorResponseDto(result.ErrorCode, result.ErrorMessage);

            if (result.ErrorCode == "404")
                return NotFound(error);

            return BadRequest(error);
        }
    }
}
