using backend_issue_nest.Models;
using System.Data.SqlClient;

namespace backend_issue_nest.Repositories
{
    public class TicketRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public TicketRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        private T GetValueOrDefault<T>(object value, T defaultValue = default)
        {
            return value != DBNull.Value ? (T)value : defaultValue;
        }


        public List<Ticket> GetTicket()
        {
            List<Ticket> tickets = new List<Ticket>();


            try
            {
                string query = "SELECT * FROM tr_tickets";

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tickets.Add(new Ticket
                                {
                                    Id = GetValueOrDefault<int>(reader["pk_tr_tickets"], 0),
                                    title = GetValueOrDefault<string>(reader["title"], string.Empty),
                                    description = GetValueOrDefault<string>(reader["description"], string.Empty),
                                    status = GetValueOrDefault<string>(reader["status"], string.Empty),
                                    client_id = GetValueOrDefault<int>(reader["client_id"], 0),
                                    admin_response = GetValueOrDefault<string>(reader["admin_response"], string.Empty),
                                    created_at = GetValueOrDefault<DateTime>(reader["created_at"], DateTime.MinValue),
                                    updated_at = GetValueOrDefault<DateTime>(reader["updated_at"], DateTime.MinValue)
                                });
                            }
                        }

                    };
                };

                return tickets;
            } catch (Exception ex)
            {
                throw;
            }
            
        }
    }
}
