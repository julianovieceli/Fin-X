using Fin_X.Domain;
using Personal.Common.Infra.MongoDb.Repository;

namespace Fin_X.Infra.MongoDb.Domain
{
    public class PatientDocument: MongoDbEntityBase
    {
        public string Name { get; set; }
        public string Docto { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime CreateDate { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime? DeletedDate { get; set; }

        public static PatientDocument FromDomain(Patient patient)
        {
            return new PatientDocument
            {
                Name = patient.Name,
                Docto = patient.Docto,
                BirthDate = patient.BirthDate.Date,
                CreateDate = patient.CreateDate,
                PhoneNumber = patient.PhoneNumber
            };
        }

        public Patient ToDomain()
        {
            Patient patient = new Patient(this.Name, this.Docto, this.BirthDate, this.PhoneNumber);
            patient.DocumentId = this.Id.ToString();

            return patient;
        }


    }
}
