using System.Linq.Expressions;

namespace Fin_X.Domain.Interfaces
{
    public interface IExamRepository
    {
        Task<List<Exam>> GetAllExamsAsync();

        Task<Exam?> GetExamByCodeAsync(string code);
    }
}
