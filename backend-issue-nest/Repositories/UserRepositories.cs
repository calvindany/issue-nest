﻿using backend_issue_nest.Models;
using backend_issue_nest.Repositories.Services;
using System.Data.SqlClient;

namespace backend_issue_nest.Repositories
{
    public class UserRepositories
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public UserRepositories(IConfiguration configuration) 
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        private T GetValueOrDefault<T>(object value, T defaultValue = default)
        {
            return value != DBNull.Value ? (T)value : defaultValue;
        }

        public async Task<List<User>> GetUser(Filter filter)
        {
            List<User> users = new List<User>();

            string query = "SELECT * FROM ms_users";

            // Construct Query base on passed filter
            if (filter.id > 0 && filter.name != null && filter.name != "")
            {
                query += " WHERE pk_ms_user = @pk_ms_user AND name LIKE @name";
            }
            else if(filter.id > 0)
            {
                query += " WHERE pk_ms_user = @pk_ms_user";
            }
            else if (filter.name != null && filter.name != "")
            {
                query += " WHERE name LIKE @name";
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (filter.id > 0 && filter.name != null && filter.name != "")
                    {
                        command.Parameters.AddWithValue("@pk_ms_user", filter.id);
                        command.Parameters.AddWithValue("@name", "%" + filter.name + "%");
                    }
                    else if (filter.id > 0) 
                    {
                        command.Parameters.AddWithValue("@pk_ms_user", filter.id);
                    }
                    else if (filter.name != null && filter.name != "")
                    {
                        command.Parameters.AddWithValue("@name", "%" + filter.name + "%");
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                id = GetValueOrDefault<int>(reader["pk_ms_user"], 0),
                                name = GetValueOrDefault<string>(reader["name"], string.Empty),
                                email = GetValueOrDefault<string>(reader["email"], string.Empty),
                                role = GetValueOrDefault<string>(reader["role"], string.Empty)
                            });
                        }

                        return users;
                    }
                }
            }
        }
        
        public async Task<string> CreateUser(User user)
        {
            string query = "INSERT INTO ms_users (name, email, role, password) VALUES (@name, @email, @role, @password);";
            string searchForExistedEmail = "SELECT COUNT(*) FROM ms_users WHERE email = @email";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(searchForExistedEmail, connection)) 
                    {
                        command.Parameters.AddWithValue("@email", user.email);

                        object? temp = await command.ExecuteScalarAsync();
                        int count = (int)temp;

                        if(count > 0)
                        {
                            return "Email already exists";
                        }
                    }

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        PasswordService ps = new PasswordService();
                        string hashedPassword = ps.HashPassword(user.password);

                        command.Parameters.AddWithValue("@name", user.name);
                        command.Parameters.AddWithValue("@email", user.email);
                        command.Parameters.AddWithValue("@role", user.role);
                        command.Parameters.AddWithValue("@password", hashedPassword);

                        await command.ExecuteNonQueryAsync();

                        return "Success create user";
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
