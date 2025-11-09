using System.ComponentModel.DataAnnotations;

namespace Fin_X.Dto
{
    public class RegisterPatientHistoryDto
    {
        public string PatientId { get; set; }

        public string? Diagnostic { get; set; }

        public string? Prescription { get; set; }

        [RequiredEnum(ErrorMessage = "PlaceId is required. Valid places: 1=Clinic, 2= Laboratory, 3=Hospital")]
        public PlaceId? PlaceId { get; set; }

        public IList<string>? Exams { get; set; }

}
}
