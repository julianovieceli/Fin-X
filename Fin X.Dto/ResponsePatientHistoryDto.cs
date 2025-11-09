namespace Fin_X.Dto
{
    public class ResponsePatientHistoryDto
    {
        public string Id { get; set; }

        public string? Diagnostic { get; set; }

        public string? Prescription { get; set; }

        public PatientHistoryPlacementId Placement { get; set; }

    }
}
