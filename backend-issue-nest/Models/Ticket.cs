﻿namespace backend_issue_nest.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public Constants.TICKET_STATUS status { get; set; }
        public string status_name { get; set; }
        public int client_id { get; set; }
        public string client_name { get; set; }
        public string admin_response {  get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set;}
    }

    public class ClientRequestCreateTicket
    {
        public string title { get; set; }
        public string description { get; set; }
        public Constants.TICKET_STATUS status { get; set; }
        public int client_id { get; set; }
        public DateTime created_at { get; set; }

    }

    public class AdminRequestResponseTicket
    {
        public string admin_response { get; set; }
        public Constants.TICKET_STATUS status { get; set; }
    }
}
