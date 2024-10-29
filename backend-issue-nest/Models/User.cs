namespace backend_issue_nest.Models
{
    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string role { get; set; }
        public string password { get; set; }
        public DateTime created_at { get; set; }
    }

    public class LoginRequest
    {
        public string email { get; set; }
        public string password { get; set; }

    }

    public class LoginResponse
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string role { get; set; }
        public string token {  get; set; }
    }
}
