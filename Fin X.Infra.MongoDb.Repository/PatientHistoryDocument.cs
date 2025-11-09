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

        public IList<ExamDocument>? Exams { get; set; }

        public DateTime CreateDate { get; set; }

        public PatientHistoryPlacementId Placement { get; set; }

        public static PatientHistoryDocument FromDomain(PatientHistory patientHistory)
        {
            return new PatientHistoryDocument
            {
                PatientDocumentId = ObjectId.Parse(patientHistory.Patient.DocumentId),

                Diagnostic = patientHistory.Diagnostic,
                Prescription = patientHistory.Prescription,

                CreateDate = patientHistory.CreateTime,

                Placement = patientHistory.Placement,

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
                Placement = this.Placement,
                Exams = this.Exams?.Select(examDoc => examDoc.ToDomain()).ToList()
            };


            patientHistory.DocumentId = this.Id.ToString();

            return patientHistory;
        }


    }
}
