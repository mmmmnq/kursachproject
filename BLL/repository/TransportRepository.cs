using DTO;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public class TransportRepository
    {
        private readonly string _connectionString;

        public TransportRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<decimal> GetTransportPriceAsync(int tourId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "SELECT t.Price " +
                    "FROM Transport t " +
                    "JOIN Tours tr ON t.TransportId = tr.TransportId " +
                    "WHERE tr.TourId = @TourId", connection))
                {
                    command.Parameters.AddWithValue("@TourId", tourId);
                    var result = await command.ExecuteScalarAsync();
                    return result == null ? 0 : Convert.ToDecimal(result);
                }
            }
        }
        public async Task<List<TransportDTO>> GetAllTransportsAsync()
        {
            var transports = new List<TransportDTO>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("SELECT * FROM Transport", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            transports.Add(new TransportDTO
                            {
                                TransportId = reader.GetInt32(0),
                                Type = reader.GetString(1),
                                Capacity = reader.GetInt32(2),
                                Company = reader.GetString(3),
                                Price = reader.GetDecimal(4)
                            });
                        }
                    }
                }
            }

            return transports;
        }
        public async Task AddTransportAsync(TransportDTO transport)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "INSERT INTO Transport (Type, Capacity, Company, Price) " +
                    "VALUES (@Type, @Capacity, @Company, @Price)", connection))
                {
                    command.Parameters.AddWithValue("@Type", transport.Type);
                    command.Parameters.AddWithValue("@Capacity", transport.Capacity);
                    command.Parameters.AddWithValue("@Company", transport.Company);
                    command.Parameters.AddWithValue("@Price", transport.Price);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateTransportAsync(TransportDTO transport)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "UPDATE Transport SET Type = @Type, Capacity = @Capacity, Company = @Company, Price = @Price " +
                    "WHERE TransportId = @TransportId", connection))
                {
                    command.Parameters.AddWithValue("@TransportId", transport.TransportId);
                    command.Parameters.AddWithValue("@Type", transport.Type);
                    command.Parameters.AddWithValue("@Capacity", transport.Capacity);
                    command.Parameters.AddWithValue("@Company", transport.Company);
                    command.Parameters.AddWithValue("@Price", transport.Price);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task DeleteTransportAsync(int transportId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(
                    "DELETE FROM Transport WHERE TransportId = @TransportId", connection))
                {
                    command.Parameters.AddWithValue("@TransportId", transportId);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}