﻿using backend_issue_nest.Models;
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


        public List<Ticket> GetTicket(string pk_tr_tickets, string user_id, string role)
        {
            List<Ticket> tickets = new List<Ticket>();
            bool isClient = role == Constants.USER_ROLE_NAME[(int)Constants.USER_ROLE.USER_ROLE_CLIENT];
            bool isAdmin = role == Constants.USER_ROLE_NAME[(int)Constants.USER_ROLE.USER_ROLE_ADMIN];

            try
            {
                string query = "SELECT * FROM tr_tickets JOIN ms_users ON tr_tickets.client_id = ms_users.pk_ms_user";

                if(isClient)
                {
                    query += " WHERE client_id = @client_id";

                    // For Get DEtail By ID Options
                    if (pk_tr_tickets != null)
                    {
                        query += " AND pk_tr_tickets = @pk_tr_tickets";
                    }
                } 
                else
                {
                    // For Get DEtail By ID Options
                    if (pk_tr_tickets != null)
                    {
                        query += " WHERE pk_tr_tickets = @pk_tr_tickets";
                    }
                }

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if(isClient)
                        {
                            command.Parameters.AddWithValue("@client_id", user_id);

                            // For Get DEtail By ID Options
                            if (pk_tr_tickets != null)
                            {
                                command.Parameters.AddWithValue("@pk_tr_tickets", pk_tr_tickets);
                            }
                        }
                        else
                        {
                            // For Get DEtail By ID Options
                            if (pk_tr_tickets != null)
                            {
                                command.Parameters.AddWithValue("@pk_tr_tickets", pk_tr_tickets);
                            }
                        }

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
                                    client_name = GetValueOrDefault<string>(reader["name"], string.Empty),
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

        public async Task<Ticket> CreateTicket(Ticket ticket, string user_id)
        {
            try
            {
                DateTime now = DateTime.Now;
                ticket.created_at = now;

                string query = "INSERT INTO tr_tickets (title, description, status, client_id, created_at) " +
                    "VALUES " +
                    "(@title, @description, @status, @client_id, @created_at)";

                using(SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@title", ticket.title);
                        command.Parameters.AddWithValue("@description", ticket.description);
                        command.Parameters.AddWithValue("@status", ticket.status);
                        command.Parameters.AddWithValue("@client_id", user_id);
                        command.Parameters.AddWithValue("@created_at", now);

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

        public async Task<Ticket> UpdateTicket(Ticket ticket, string user_id, string role)
        {
            try
            {
                Ticket updatedTicket = null;
                string getTickets = "SELECT client_id FROM tr_tickets WHERE pk_tr_tickets = @pk_tr_tickets ";
                string query = "UPDATE tr_tickets SET " +
                    "title = @title," +
                    "description = @description," +
                    "status = @status," +
                    "updated_at = @updated_at " +
                    "WHERE pk_tr_tickets = @pk_tr_tickets";

                bool isClient = role == Constants.USER_ROLE_NAME[(int)Constants.USER_ROLE.USER_ROLE_CLIENT];

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    if(isClient)
                    {
                        using (SqlCommand cmd = new SqlCommand(getTickets, connection))
                        {
                            cmd.Parameters.AddWithValue("@pk_tr_tickets", ticket.Id);

                            object? temp = await cmd.ExecuteScalarAsync();
                            int client_id = (int)temp;

                            if (client_id != Convert.ToInt32(user_id))
                            {
                                return null;
                            }
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@title", ticket.title);
                        cmd.Parameters.AddWithValue("@description", ticket.description);
                        cmd.Parameters.AddWithValue("@status", ticket.status);
                        cmd.Parameters.AddWithValue("@pk_tr_tickets", ticket.Id);
                        cmd.Parameters.AddWithValue("@updated_at", DateTime.Now);

                        await cmd.ExecuteNonQueryAsync();
                    }

                    query = "SELECT * FROM tr_tickets WHERE pk_tr_tickets = @pk_tr_tickets ";
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

        public async Task<Ticket> UpdateResponseTicket(Ticket ticket, string user_id, string role)
        {
            try
            {
                Ticket updatedTicket = null;
                string getTickets = "SELECT * FROM tr_tickets WHERE pk_tr_tickets = @pk_tr_tickets ";
                string query = "UPDATE tr_tickets SET " +
                    "admin_response = @admin_response, updated_at = @updated_at " +
                    "WHERE pk_tr_tickets = @pk_tr_tickets";

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@admin_response", ticket.admin_response);
                        cmd.Parameters.AddWithValue("@updated_at", DateTime.Now);
                        cmd.Parameters.AddWithValue("@pk_tr_tickets", ticket.Id);

                        await cmd.ExecuteNonQueryAsync();
                    }

                    using (SqlCommand cmd = new SqlCommand(getTickets, connection))
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
                throw ex;
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
