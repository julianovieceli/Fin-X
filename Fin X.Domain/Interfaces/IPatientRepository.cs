using System.Linq.Expressions;

namespace Fin_X.Domain.Interfaces
{
    public interface IPatientRepository
    {
        Task<bool> InsertAsync(Patient patient);

        Task<Patient> GetByDocto(string docto);

    }
}
