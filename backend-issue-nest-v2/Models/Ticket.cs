using System.ComponentModel.DataAnnotations.Schema;

namespace backend_issue_nest_v2.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int UserId {  get; set; }
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
        public string AdminResponse { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
