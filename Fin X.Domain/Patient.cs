using Personal.Common.Domain;
using Personal.Common.Domain.Validators;

namespace Fin_X.Domain
{
    public class Patient: BaseDomain
    {
        private string _name;

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

        public string Docto { get; init; }

        public DateTime BirthDate { get; init; }

        public string? PhoneNumber { get; set; }

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

    }


    
}
