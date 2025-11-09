using MongoDB.Bson;
using Personal.Common.Infra.MongoDb.Repository;

namespace Fin_X.Domain
{
    public class ExamDocument: MongoDbEntityBase
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public static ExamDocument FromDomain(Exam exam)
        {
            return new ExamDocument
            {
                Code = exam.Code,
                Name = exam.Name,
                Id = ObjectId.Parse(exam.DocumentId)
            };
        }

        public Exam ToDomain()
        {
            Exam exam = new Exam()
            {
                Name = this.Name,
                Code = this.Code,
                DocumentId = this.Id.ToString()
            };


            return exam;
        }
    }
}