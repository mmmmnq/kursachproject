using System;
using Npgsql;
using kursovaya.Models;

namespace kursovaya.Services
{
    public class RegisterService
    {
        private readonly string _connectionString;

        public RegisterService(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Строка подключения не может быть пустой.", nameof(connectionString));

            _connectionString = connectionString;
        }

        /// <summary>
        /// Регистрирует нового пользователя в системе.
        /// </summary>
        /// <param name="user">Модель пользователя с данными для регистрации.</param>
        /// <param name="errorMessage">Сообщение об ошибке, если регистрация не удалась.</param>
        /// <returns>true, если регистрация успешна; false в случае ошибки.</returns>
        public bool RegisterUser(User user, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (user == null)
            {
                errorMessage = "Пользователь не может быть пустым.";
                return false;
            }

            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

                    // SQL-запрос для вставки данных в таблицу
                    const string query = @"INSERT INTO users 
                                           (username, password, role, firstname, secondname, phonenumber) 
                                           VALUES (@username, @password, 'user', @firstname, @secondname, @phonenumber)";

                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        // Добавление параметров для предотвращения SQL-инъекций
                        cmd.Parameters.AddWithValue("username", user.Username);
                        cmd.Parameters.AddWithValue("password", user.Password);
                        cmd.Parameters.AddWithValue("firstname", user.FirstName);
                        cmd.Parameters.AddWithValue("secondname", user.LastName); // secondname соответствует базе
                        cmd.Parameters.AddWithValue("phonenumber", user.Phone);
                        

                        // Выполнение SQL-запроса
                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (NpgsqlException npgEx)
            {
                // Ошибки, связанные с PostgreSQL
                errorMessage = $"Ошибка базы данных: {npgEx.Message}";
            }
            catch (Exception ex)
            {
                // Общие ошибки
                errorMessage = $"Неизвестная ошибка: {ex.Message}";
            }

            return false;
        }
    }
}
