namespace Fin_X.Dto
{
    public class ResponsePatientDto
    {

        public string Id { get; set; }
        public string Name { get; set; }
        
        public string Docto { get; set; }

        public DateTime BirthDate { get; init; }

        public string? PhoneNumber { get; set; }
    }
}
