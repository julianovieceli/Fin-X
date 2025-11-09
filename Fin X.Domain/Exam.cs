using Personal.Common.Domain;

namespace Fin_X.Domain
{
    public class Exam: BaseDomain
    {
        public string DocumentId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}