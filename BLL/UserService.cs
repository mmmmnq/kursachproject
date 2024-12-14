using Npgsql;
using System;

namespace BLL
{
    public class UserService : IUserService
    {
        private string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=123;Database=TravelAgency"; // Подключение к базе данных PostgreSQL

        public bool AuthenticateUser(string username, string password, out string role)
        {
            role = null;

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL-запрос для поиска пользователя по имени и паролю
                    string query = "SELECT userid, role FROM users WHERE username = @username AND password = @password LIMIT 1";

                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        // Параметры для SQL-запроса
                        cmd.Parameters.AddWithValue("username", username);
                        cmd.Parameters.AddWithValue("password", password);

                        
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                
                                int userId = reader.GetInt32(0); 
                                role = reader.GetString(1);            

                                
                                CurrentUser.SetUser(userId, username, role);

                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                Console.WriteLine($"Ошибка авторизации: {ex.Message}");
            }

            return false;
        }
        public int GetUserId(string username)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    
                    string query = "SELECT userid FROM users WHERE username = @username LIMIT 1";

                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("username", username);

                        var result = cmd.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int userId))
                        {
                            return userId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка получения ID пользователя: {ex.Message}");
            }

            return 0; 
        }

    }
}
