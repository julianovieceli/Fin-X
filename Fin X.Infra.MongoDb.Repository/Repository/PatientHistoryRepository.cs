using Fin_X.Domain;
using Fin_X.Domain.Interfaces;
using Fin_X.Infra.MongoDb.Domain;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Personal.Common.Infra.MongoDb.Repository;
using Personal.Common.Infra.MongoDb.Repository.Interfaces;

namespace Fin_X.Infra.MongoDb.Repository.Repository
{
    public class PatientHistoryRepository : MongoDbRepositoryBase<PatientHistoryDocument>, IPatientHistoryRepository
    {
        public PatientHistoryRepository(IMongoDbcontext dbcontext, ILogger<PatientHistoryDocument> logger)
            : base(dbcontext, "PatientHistory", logger)
        {

        }

        public async Task<PatientHistory> InsertAsync(PatientHistory patientHistory)
        {
            try
            {
                PatientHistoryDocument patientHistoryDocumentToInsert = PatientHistoryDocument.FromDomain(patientHistory);

                await base.InsertAsync(patientHistoryDocumentToInsert);

                return patientHistoryDocumentToInsert.ToDomain();
            }
            catch
            {
                throw;
            }
        }


        public async Task<IList<PatientHistory>> GetHistoryByPatientId(string patientDocumentId)
        {
            try
            {
                var patientHistoryDocumentList = await base.FindAsync(d => d.PatientDocumentId.ToString() == patientDocumentId);


                return patientHistoryDocumentList.Select(p => p.ToDomain()).ToList();
            }
            catch
            {
                throw;
            }
        }
    }

   
    
}
