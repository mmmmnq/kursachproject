using Npgsql;
using System;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public class EmailRepository
    {
        private readonly string _connectionString;

        public EmailRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task UpdateUserEmailAsync(int userId, string email)
        {
            const string query = "UPDATE Users SET Email = @Email WHERE UserId = @UserId";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@UserId", userId);
                    int affectedRows = await command.ExecuteNonQueryAsync();

                    if (affectedRows == 0)
                    {
                        throw new Exception($"Пользователь с ID {userId} не найден.");
                    }
                }
            }
        }
    }
}
