using Personal.Common.Domain;

namespace Fin_X.Application.Api
{
    public interface IBrasilApiService
    {
        Task<Result> GetAddress(string cep);
    }
}
