using Fin_X.Application.Services;
using Fin_X.Dto;
using Microsoft.AspNetCore.Mvc;
using Personal.Common.Domain;
using Porter.Api.Controllers;

namespace Fin_X.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController : CustomBaseController
    {
        private readonly IPatientService _patientServive;

        public PatientController(ILogger<PatientController> logger, IPatientService patientServive) : base(logger)
        {
            _patientServive = patientServive;
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
    }
}
