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
    public class PatientRepository : MongoDbRepositoryBase<PatientDocument>, IPatientRepository
    {
        public PatientRepository(IMongoDbcontext dbcontext, ILogger<PatientDocument> logger)
            :base(dbcontext, "Patient", logger)
        {
            
        }

        public async Task<Patient> InsertAsync(Patient patient)
        {
            try
            {
                PatientDocument patientDocumentToInsert = PatientDocument.FromDomain(patient);

                await base.InsertAsync(patientDocumentToInsert);

                return patientDocumentToInsert.ToDomain();
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
                var patientDocument = await base.FindOneAsync(d => d.Docto == docto && !d.DeletedDate.HasValue);

                if (patientDocument is not null)
                    return patientDocument.ToDomain();

                return null;
                
            }
            catch 
            {
                throw;
            }
        }

        public async Task<List<Patient>> GetAllPatientsAsync()
        {
            try
            {
                var patientDocumentList = await base.GetAllAsync();

                patientDocumentList = patientDocumentList.Where(p => !p.DeletedDate.HasValue).ToList();

                return patientDocumentList.Select( p => p.ToDomain()).ToList();
            }
            catch
            {
                throw;
            }
        }

        public async Task<long> GetCountByDocto(string docto)
        {
            try
            {
                return await base.CountAsync(d => d.Docto == docto && !d.DeletedDate.HasValue);

            }
            catch
            {
                throw;
            }
        }

        public async Task<long> Delete(string Id)
        {
            try
            {
                ObjectId objectId = ObjectId.Parse(Id);

                var filter = Builders<PatientDocument>.Filter.Eq(d => d.Id, objectId);
                filter = filter & Builders<PatientDocument>.Filter.Eq(d => d.DeletedDate, null);


                var update = Builders<PatientDocument>.Update.Set(d => d.DeletedDate, DateTime.Now);

                // 3. Execute the update operation
                var result = await _collection.UpdateOneAsync(filter, update);

                if (result.ModifiedCount > 0)
                    _logger.LogInformation($"Paciente excluido com sucesso!");
                else
                    _logger.LogInformation($"Nenhum paciente excluido!");

                return result.ModifiedCount;
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                _logger.LogError(ex, "Erro ao excluir uma reserva.");
                throw;
            }
        }
    }

   
    
}
