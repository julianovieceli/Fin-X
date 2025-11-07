using System.Linq.Expressions;

namespace Fin_X.Domain.Interfaces
{
    public interface IPatientRepository
    {
        Task<Patient> InsertAsync(Patient patient);

        Task<Patient> GetByDocto(string docto);

        Task<long> GetCountByDocto(string docto);

        Task<long> Delete(string documentId);

        Task<List<Patient>> GetAllPatientsAsync();

    }
}
