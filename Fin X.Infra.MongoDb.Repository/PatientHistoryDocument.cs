using Fin_X.Domain;
using MongoDB.Bson;
using Personal.Common.Infra.MongoDb.Repository;

namespace Fin_X.Infra.MongoDb.Domain
{
    public class PatientHistoryDocument : MongoDbEntityBase
    {
        public ObjectId PatientDocumentId { get; set; }

        public string? Diagnostic { get; set; }

        public string? Prescription { get; set; }

        public string User { get; set; }

        public IList<ExamDocument>? Exams { get; set; }

        public DateTime CreateDate { get; set; }

        public PlaceId PlaceId { get; set; }

        public static PatientHistoryDocument FromDomain(PatientHistory patientHistory)
        {
            return new PatientHistoryDocument
            {
                PatientDocumentId = ObjectId.Parse(patientHistory.Patient.DocumentId),

                Diagnostic = patientHistory.Diagnostic,
                Prescription = patientHistory.Prescription,

                CreateDate = patientHistory.CreateTime,

                PlaceId = patientHistory.PlaceId,

                User = patientHistory.User,

                Exams = patientHistory.Exams?.Select(exam => ExamDocument.FromDomain(exam)).ToList()

            };
        }

        public PatientHistory ToDomain()
        {
            PatientHistory patientHistory = new PatientHistory()
            {
                Diagnostic = this.Diagnostic,
                Prescription = this.Prescription,
                CreateTime = this.CreateDate,
                PlaceId = this.PlaceId,
                User = this.User,
                Exams = this.Exams?.Select(examDoc => examDoc.ToDomain()).ToList()
            };


            patientHistory.DocumentId = this.Id.ToString();

            return patientHistory;
        }


    }
}
