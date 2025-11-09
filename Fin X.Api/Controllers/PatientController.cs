using Fin_X.Application.Services;
using Fin_X.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personal.Common.Domain;
using Porter.Api.Controllers;

namespace Fin_X.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]

    public class PatientController : CustomBaseController
    {
        private readonly IPatientService _patientServive;

        public PatientController(ILogger<PatientController> logger, IPatientService patientServive) : base(logger)
        {
            _patientServive = patientServive;
        }

        [HttpGet("fetch-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _patientServive.GetAllPatientsAsync();


            if (result.IsFailure)
            {
                return base.CreateResponseFromResult(result);
            }

            return Ok(Result<IList<ResponsePatientDto>>.Success(((Result<IList<ResponsePatientDto>>)result).Response));
        }

        [HttpGet()]
        public async Task<IActionResult> Get([FromQuery] string docto)
        {
            var result = await _patientServive.Get(docto);


            if (result.IsFailure)
            {
                return base.CreateResponseFromResult(result);
            }

            return Ok(Result<ResponsePatientDto>.Success(((Result<ResponsePatientDto>)result).Response));
        }

        [HttpPost(Name = "PostPatient")]
        public async Task<IActionResult> Register(RegisterPatientDto registerPatient)
        {
            var result = await _patientServive.RegisterAsync(registerPatient);


            if (result.IsFailure)
            {
                return base.CreateResponseFromResult(result);

            }
            return StatusCode(StatusCodes.Status201Created, ((Result<ResponsePatientDto>)result).Response);
        }


        [HttpDelete(Name = "DeletePatient")]
        public async Task<IActionResult> Delete([FromQuery] string Id)
        {
            var result = await _patientServive.Delete(Id);


            if (result.IsFailure)
            {
                return base.CreateResponseFromResult(result);

            }
            return Ok();
        }



        [HttpPost("history/", Name = "PostPatientHistory")]
        public async Task<IActionResult> RegisterHistory(RegisterPatientHistoryDto registerPatientHistoryDto)
        {
            var username = User.Identity?.Name;

            var result = await _patientServive.RegisterPatientHistoryAsync(username, registerPatientHistoryDto);


            if (result.IsFailure)
            {
                return base.CreateResponseFromResult(result);

            }
            return StatusCode(StatusCodes.Status201Created, ((Result<ResponsePatientHistoryDto>)result).Response);
        }

    }
}
