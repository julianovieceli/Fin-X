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

        public PatientHistoryPlacementId Placement { get; set; }

        public string User { get; init; }

        public PatientHistory()
        {
            
        }
        public PatientHistory(Patient patient, string user, string? diagnostic, string? prescription, PatientHistoryPlacementId placement, IList<Exam>? exams)
        {
            ArgumentNullException.ThrowIfNull(patient, nameof(patient));

            Patient = patient;

            if(string.IsNullOrWhiteSpace(diagnostic) && string.IsNullOrWhiteSpace(prescription) && (exams is null || exams.Count == 0))
            {
                throw new ArgumentException("Ao menos um dos campos diagnóstico, prescrição ou exames deve ser preenchido.");
            }

            if(string.IsNullOrWhiteSpace(user))
                throw new ArgumentException("Usuário que cria o histórico obrigatório");

            User = user;

            Diagnostic = diagnostic;
            Prescription = prescription;
            this.Exams = Exams;
            Placement = placement;  

            base.CreateTime = DateTime.Now;

        }
    }


    
}
