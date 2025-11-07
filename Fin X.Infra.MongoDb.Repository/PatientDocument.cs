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

        public static PatientDocument FromDomain(Patient patient)
        {
            return new PatientDocument
            {
                Name = patient.Name,
                Docto = patient.Docto,
                BirthDate = patient.BirthDate
            };
        }

        public Patient ToDomain()
        {
            return new Patient(this.Name, this.Docto, this.BirthDate, this.PhoneNumber);
        }


    }
}
