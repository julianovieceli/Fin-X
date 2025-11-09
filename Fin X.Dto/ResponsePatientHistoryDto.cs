namespace Fin_X.Dto
{
    public class ResponsePatientHistoryDto
    {
        public string Id { get; set; }

        public string? Diagnostic { get; set; }

        public string? Prescription { get; set; }

        public PlaceId PlaceId { get; set; }

        public string PlaceDescription { get; set; }

        public DateTime RegisterDate { get; set; }

        public IList<string>? Exams { get; set; }

    }
}
