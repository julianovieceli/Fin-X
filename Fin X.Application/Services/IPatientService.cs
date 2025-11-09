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


        Task<Result> RegisterPatientHistoryAsync(string user, RegisterPatientHistoryDto patient);

        Task<Result> GetHistoryByPatientId(string patientId);


        Task<Result> GetAddressExternalApi(string cep);

    }
}
