using Fin_X.Domain;
using Fin_X.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Personal.Common.Infra.MongoDb.Repository;
using Personal.Common.Infra.MongoDb.Repository.Interfaces;

namespace Fin_X.Infra.MongoDb.Repository.Repository
{
    public class ExamRepository : MongoDbRepositoryBase<ExamDocument>, IExamRepository
    {
        public ExamRepository(IMongoDbcontext dbcontext, ILogger<ExamDocument> logger)
            :base(dbcontext, "Exam", logger)
        {
            
        }

        public async Task<List<Exam>> GetAllExamsAsync()
        {
            try
            {
                var examDocumentList = await base.GetAllAsync();

                return examDocumentList.Select( p => p.ToDomain()).ToList();
            }
            catch
            {
                throw;
            }
        }

        public async Task<Exam?> GetExamByCodeAsync(string code)
        {
            try
            {
                var examDocument = await base.FindOneAsync(p => p.Code == code);

                if(examDocument is not null)
                    return examDocument.ToDomain();
                return null;
            }
            catch
            {
                throw;
            }
        }
    }

   
    
}
