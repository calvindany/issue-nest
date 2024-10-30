namespace backend_issue_nest.Models
{
    public class Ticket
    {
        public int? Id { get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public string? status { get; set; }
        public int? client_id { get; set; }
        public string? admin_response {  get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set;}
    }
}
