namespace Fin_X.Dto
{

    public class ResponseErrorAddressDto
    {
        public string name { get; set; }
        public string message { get; set; }
        public string type { get; set; }
        public Error[] errors { get; set; }
    }

    public class Error
    {
        public string message { get; set; }
        public string service { get; set; }
    }

}
