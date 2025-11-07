using System.ComponentModel.DataAnnotations;

namespace Fin_X.Dto
{
    public class RegisterPatientDto
    {

        [Required(ErrorMessage = "Nome obrigatório.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Docto obrigatório.")]
        public string Docto { get; set; }

        [Required(ErrorMessage = "Data Nascimento obrigatorio.")]
        public DateTime BirthDate { get; init; }

        [Phone(ErrorMessage = "Numero de telefone inválido")]
        public string? PhoneNumber { get; set; }
    }
}
