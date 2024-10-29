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

        public async Task<Ticket> CreateTicket(Ticket ticket)
        {
            try
            {
                string query = "INSERT INTO tr_tickets (title, description, status, client_id) " +
                    "VALUES " +
                    "(@title, @description, @status, @client_id)";

                using(SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@title", ticket.title);
                        command.Parameters.AddWithValue("@description", ticket.description);
                        command.Parameters.AddWithValue("@status", ticket.status);
                        command.Parameters.AddWithValue("@client_id", ticket.client_id);

                        await command.ExecuteNonQueryAsync();

                        return ticket;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Ticket> UpdateTicket(Ticket ticket)
        {
            try
            {
                Ticket updatedTicket = null;
                string query = "UPDATE tr_tickets SET " +
                    "title = @title," +
                    "description = @description," +
                    "status = @status " +
                    "WHERE pk_tr_tickets = @pk_tr_tickets";

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@title", ticket.title);
                        cmd.Parameters.AddWithValue("@description", ticket.description);
                        cmd.Parameters.AddWithValue("@status", ticket.status);
                        cmd.Parameters.AddWithValue("@pk_tr_tickets", ticket.Id);

                        await cmd.ExecuteNonQueryAsync();
                    }

                    query = "SELECT * FROM tr_tickets WHERE pk_tr_tickets = @pk_tr_tickets";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@pk_tr_tickets", ticket.Id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            await reader.ReadAsync();
                            updatedTicket = new Ticket
                            {
                                Id = GetValueOrDefault<int>(reader["pk_tr_tickets"], 0),
                                title = GetValueOrDefault<string>(reader["title"], string.Empty),
                                description = GetValueOrDefault<string>(reader["description"], string.Empty),
                                status = GetValueOrDefault<string>(reader["status"], string.Empty),
                                client_id = GetValueOrDefault<int>(reader["client_id"], 0),
                                admin_response = GetValueOrDefault<string>(reader["admin_response"], string.Empty),
                                created_at = GetValueOrDefault<DateTime>(reader["created_at"], DateTime.MinValue),
                                updated_at = GetValueOrDefault<DateTime>(reader["updated_at"], DateTime.MinValue)
                            };

                            return updatedTicket;
                        }
                    }
                }
            } 
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> DeleteTicket(int id)
        {
            try
            {
                string query = "DELETE FROM tr_tickets WHERE pk_tr_tickets = @pk_tr_tickets";
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand cmd  = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@pk_tr_tickets", id);

                        await cmd.ExecuteNonQueryAsync();

                        return id;
                    }
                }
            } 
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
