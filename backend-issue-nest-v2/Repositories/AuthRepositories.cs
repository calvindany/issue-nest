using backend_issue_nest.Models;
using backend_issue_nest.Repositories.Services;
using System.Data.SqlClient;

namespace backend_issue_nest.Repositories
{
    public class AuthRepositories
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public AuthRepositories(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        private T GetValueOrDefault<T>(object value, T defaultValue = default)
        {
            return value != DBNull.Value ? (T)value : defaultValue;
        }

        public async Task<User> Login(string email, string password)
        {
            try
            {
                string query = "SELECT * FROM ms_users WHERE email = @email";
                User loggedUser = null;

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@email", email);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if(await reader.ReadAsync())
                            {
                                loggedUser = new User
                                {
                                    id = GetValueOrDefault<int>(reader["pk_ms_user"], 0),
                                    name = GetValueOrDefault<string>(reader["name"], string.Empty),
                                    email = GetValueOrDefault<string>(reader["email"], string.Empty),
                                    role = (Constants.USER_ROLE)Constants.GetUserRoleIndex(GetValueOrDefault<string>(reader["role"], string.Empty)) + 1,
                                    role_name = GetValueOrDefault<string>(reader["role"], string.Empty),
                                    password = GetValueOrDefault<string>(reader["password"], string.Empty),
                                    is_active = GetValueOrDefault<bool>(reader["is_active"], false),
                                };

                                if(loggedUser.id == 0)
                                {
                                    return null;
                                }

                                PasswordService ps = new PasswordService();
                                if(ps.VerifyPassword(loggedUser.password, password))
                                {
                                    return loggedUser;
                                }

                                return null;
                            } 

                            return null;

                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }
    }
}
