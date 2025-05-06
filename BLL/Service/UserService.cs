using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DTO;
using Interfaces.Services;
using Npgsql;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly string connectionString;

        // Конструктор по умолчанию
        public UserService()
        {
            connectionString = "Host=localhost;Port=5432;Username=postgres;Password=123;Database=TravelAgency";
        }

        public UserService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool AuthenticateUser(string username, string password, out string role)
        {
            role = null;

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT userid, role FROM users WHERE username = @username AND password = @password LIMIT 1";

                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
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

        public async Task<List<UserDTO>> GetUsersAsync()
        {
            var users = new List<UserDTO>();

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Обновленный запрос, который теперь извлекает телефонный номер
                    string query = @"
                SELECT userid, username, password, email, firstname, lastname, role, phonenumber 
                FROM users";

                    using (var cmd = new NpgsqlCommand(query, connection))
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            users.Add(new UserDTO
                            {
                                UserId = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                Password = reader.GetString(2),
                                Email = reader.IsDBNull(3) ? null : reader.GetString(3),
                                FirstName = reader.IsDBNull(4) ? null : reader.GetString(4),
                                LastName = reader.IsDBNull(5) ? null : reader.GetString(5),
                                Role = reader.GetString(6),
                                PhoneNumber = reader.IsDBNull(7) ? null : reader.GetString(7) // Добавлено для phone number
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке пользователей: {ex.Message}");
            }

            return users;
        }


        public async Task<bool> AddUserAsync(UserDTO user)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    string query = @"
                INSERT INTO users (username, password, email, firstname, lastname, role)
                VALUES (@username, @password, @email, @firstname, @lastname, @role)";

                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("username", user.Username);
                        cmd.Parameters.AddWithValue("password", user.Password); // Убедитесь, что пароль хэширован
                        cmd.Parameters.AddWithValue("email", (object)user.Email ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("firstname", (object)user.FirstName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("lastname", (object)user.LastName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("role", user.Role ?? "User");

                        int affected = await cmd.ExecuteNonQueryAsync();
                        return affected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении пользователя: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> DeleteUserAsync(int userId)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    string query = "DELETE FROM users WHERE userid = @userId";

                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("userId", userId);
                        int affectedRows = await cmd.ExecuteNonQueryAsync();
                        return affectedRows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении пользователя: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> CreateUserAsync(UserDTO user)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    string query = @"
            INSERT INTO users (username, password, email, firstname, lastname, role, phonenumber)
            VALUES (@username, @password, @email, @firstname, @lastname, @role, @phonenumber)";
                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("username", user.Username);
                        cmd.Parameters.AddWithValue("password", user.Password);
                        cmd.Parameters.AddWithValue("email", (object)user.Email ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("firstname", (object)user.FirstName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("lastname", (object)user.LastName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("role", user.Role ?? "User");
                        cmd.Parameters.AddWithValue("phonenumber", (object)user.PhoneNumber ?? DBNull.Value);
                        int affected = await cmd.ExecuteNonQueryAsync();
                        return affected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании пользователя: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> UpdateUserAsync(UserDTO user)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    string query = string.IsNullOrEmpty(user.Password)
                        ? @"UPDATE users 
                SET username = @username, 
                    email = @email, 
                    firstname = @firstname, 
                    lastname = @lastname, 
                    role = @role,
                    phonenumber = @phonenumber
                WHERE userid = @userid"
                        : @"UPDATE users 
                SET username = @username, 
                    password = @password, 
                    email = @email, 
                    firstname = @firstname, 
                    lastname = @lastname, 
                    role = @role,
                    phonenumber = @phonenumber
                WHERE userid = @userid";
                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("userid", user.UserId);
                        cmd.Parameters.AddWithValue("username", user.Username);
                        if (!string.IsNullOrEmpty(user.Password))
                            cmd.Parameters.AddWithValue("password", user.Password);
                        cmd.Parameters.AddWithValue("email", (object)user.Email ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("firstname", (object)user.FirstName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("lastname", (object)user.LastName ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("role", user.Role ?? "User");
                        cmd.Parameters.AddWithValue("phonenumber", (object)user.PhoneNumber ?? DBNull.Value);
                        int affected = await cmd.ExecuteNonQueryAsync();
                        return affected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обновлении пользователя: {ex.Message}");
                return false;
            }
        }
    }

}
