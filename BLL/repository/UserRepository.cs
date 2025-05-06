//using Npgsql;
//using System;

//namespace BLL.Repositories
//{
//    public class UserRepository
//    {
//        private readonly string _connectionString;

//        public UserRepository(string connectionString)
//        {
//            _connectionString = connectionString;
//        }

//        public bool AuthenticateUser(string username, string password, out string role)
//        {
//            role = null;

//            try
//            {
//                using (var connection = new NpgsqlConnection(_connectionString))
//                {
//                    connection.Open();

//                    string query = "SELECT userid, role FROM users WHERE username = @username AND password = @password LIMIT 1";

//                    using (var cmd = new NpgsqlCommand(query, connection))
//                    {
//                        cmd.Parameters.AddWithValue("username", username);
//                        cmd.Parameters.AddWithValue("password", password);

//                        using (var reader = cmd.ExecuteReader())
//                        {
//                            if (reader.Read())
//                            {
//                                int userId = reader.GetInt32(0);
//                                role = reader.GetString(1);

//                                return true;
//                            }
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Ошибка авторизации: {ex.Message}");
//            }

//            return false;
//        }

//        public int GetUserId(string username)
//        {
//            try
//            {
//                using (var connection = new NpgsqlConnection(_connectionString))
//                {
//                    connection.Open();

//                    string query = "SELECT userid FROM users WHERE username = @username LIMIT 1";

//                    using (var cmd = new NpgsqlCommand(query, connection))
//                    {
//                        cmd.Parameters.AddWithValue("username", username);

//                        var result = cmd.ExecuteScalar();
//                        if (result != null && int.TryParse(result.ToString(), out int userId))
//                        {
//                            return userId;
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Ошибка получения ID пользователя: {ex.Message}");
//            }

//            return 0;
//        }
//    }
//}
