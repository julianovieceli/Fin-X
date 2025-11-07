using Fin_X.Dto;
using Personal.Common.Domain;

namespace Fin_X.Application.Services
{
    public interface IPatientService
    {
        Task<Result> RegisterAsync(RegisterPatientDto patient);

        Task<Result> Get(string docto);

        Task<Result> Delete(string Id);

        Task<Result> GetAllPatientsAsync();

    }
}
