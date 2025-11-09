using System.ComponentModel.DataAnnotations;

namespace Fin_X.Dto
{
    public class RegisterPatientHistoryDto
    {
        public string PatientDocumentId { get; set; }

        public string? Diagnostic { get; set; }

        public string? Prescription { get; set; }

        [RequiredEnum(ErrorMessage = "PlacementId is required. Valid places: 1=Clinic, 2= Laboratory, 3=Hospital")]
        public PatientHistoryPlacementId? PlacementId { get; set; }

}
}
