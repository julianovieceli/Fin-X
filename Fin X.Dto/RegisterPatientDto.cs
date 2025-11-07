namespace Fin_X.Dto
{
    public class RegisterPatientDto
    {

        public string Name { get; set; }
        
        public string Docto { get; set; }

        public DateTime BirthDate { get; init; }

        public string? PhoneNumber { get; set; }
    }
}
