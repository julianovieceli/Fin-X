using Personal.Common.Domain;
using Personal.Common.Domain.Validators;

namespace Fin_X.Domain
{
    public class Patient: BaseDomain
    {
        

        private string _name;

        private string _phoneNumber;
        public string Name {
            get { return _name; }
            set
            {
                if(string.IsNullOrWhiteSpace(value) || value.Length < 5)
                {
                    throw new ArgumentException("Name deve conter ao menos 5 caracteres.", nameof(Name));
                }
                _name = value;
            }
        }

        public string DocumentId { get; set;}

        public string Docto { get; init; }

        public DateTime BirthDate { get; init; }

        public DateTime? DeletedDate { get; set; }

        public string? PhoneNumber
        {
            get
            {
                return _phoneNumber;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && !PhoneValidador.IsValid(value))
                {
                    throw new ArgumentException("Telefone inválido", nameof(PhoneNumber));
                }
                _phoneNumber = value;
            }
        }

        public DateTime CreateDate { get; init; }

        public Patient(string name, string docto , DateTime birthDate, string? phoneNumber)
        {
            ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
            ArgumentException.ThrowIfNullOrEmpty(docto, nameof(docto));


            Name = name;

            if (!DocumentValidator.IsCpfCnpjValid(docto))
                throw new Exception("Invalid Document!");
            Docto = docto;

            if (birthDate.Date >= DateTime.Now.Date)
                throw new Exception("BirthDate deve ser menor que a atual!");


            
            BirthDate = birthDate;
            PhoneNumber = phoneNumber;



            CreateDate = DateTime.Now;
        }

        public void Detete()
        {
            if (this.DeletedDate.HasValue)
                throw new Exception("Paciente já está deletado!");

            this.DeletedDate = DateTime.Now;
        }

    }


    
}
