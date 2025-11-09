using Personal.Common.Domain;

namespace Fin_X.Domain
{
    public class PatientHistory : BaseDomain
    {
        public string DocumentId { get; set; }
        public Patient Patient { get; set; }

        public string? Diagnostic { get; set; }

        public string? Prescription { get; set; }

        public IList<Exam>? Exams { get; set; }

        public PatientHistoryPlacement Placement { get; set; }



        public PatientHistory()
        {
            
        }
        public PatientHistory(Patient patient, string? diagnostic, string? prescription, PatientHistoryPlacement placement, IList<Exam>? Exams)
        {
            ArgumentNullException.ThrowIfNull(patient, nameof(patient));

            Patient = patient;
            Diagnostic = diagnostic;
            Prescription = prescription;
            this.Exams = Exams;
            Placement = placement;  

            base.CreateTime = DateTime.Now;

        }
    }


    
}
