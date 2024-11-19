namespace backend_issue_nest.Models
{
    public class Response
    {
        public int status_code { get; set; }
        public string message { get; set; }
        public object result { get; set; }
        public object err { get; set; }
    }
}
