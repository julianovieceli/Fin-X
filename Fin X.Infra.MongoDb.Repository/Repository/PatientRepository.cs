using Fin_X.Domain;
using Fin_X.Domain.Interfaces;
using Fin_X.Infra.MongoDb.Domain;
using Microsoft.Extensions.Logging;
using Personal.Common.Infra.MongoDb.Repository;
using Personal.Common.Infra.MongoDb.Repository.Interfaces;

namespace Fin_X.Infra.MongoDb.Repository.Repository
{
    public class PatientRepository : MongoDbRepositoryBase<PatientDocument>, IPatientRepository
    {
        public PatientRepository(IMongoDbcontext dbcontext, ILogger<PatientDocument> logger)
            :base(dbcontext, "Patient", logger)
        {
            
        }

        public async Task<bool> InsertAsync(Patient patient)
        {
            try
            {
                return await base.InsertAsync(PatientDocument.FromDomain(patient));
            }
            catch
            {
                throw;
            }
        }

        public async Task<Patient?> GetByDocto(string docto)
        {
            try
            {
                var patientDocument = await base.FindAsync(d => d.Docto == docto);

                if (patientDocument is not null)
                    return patientDocument.ToDomain();

                return null;
                
            }
            catch 
            {
                throw;
            }
        }
    }

   
    
}
