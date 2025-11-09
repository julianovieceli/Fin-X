namespace Fin_X.Domain.Interfaces
{
    public interface IPatientHistoryRepository
    {
        Task<PatientHistory> InsertAsync(PatientHistory patientHistory);

        Task<IList<PatientHistory>> GetHistoryByPatientId(string patientDocumentId);

    }
}
