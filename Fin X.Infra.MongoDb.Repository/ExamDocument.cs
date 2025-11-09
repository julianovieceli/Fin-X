using Personal.Common.Infra.MongoDb.Repository;

namespace Fin_X.Domain
{
    public class ExamDocument: MongoDbEntityBase
    {
        public string Name { get; set; }


        public static ExamDocument FromDomain(Exam exam)
        {
            return new ExamDocument
            {
                Name = exam.Name
            };
        }

        public Exam ToDomain()
        {
            Exam exam = new Exam()
            {
                Name = this.Name
            };

            exam.DocumentId = this.Id.ToString();

            return exam;
        }
    }
}