using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DTO;
using DomainModel;

namespace BLL.Repositories
{
    public class TourRepository
    {
        private readonly string _connectionString;

        public TourRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> AddTourAsync(TourDTO tour)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var query = @"INSERT INTO Tours (name, description, price, startdate, enddate, 
                         destination, availableseats, status, transportid)
                         VALUES (@name, @description, @price, @startdate, @enddate, 
                         @destination, @availableseats, @status, @transportid)";

                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@name", tour.Name);
                        cmd.Parameters.AddWithValue("@description", tour.Description);
                        cmd.Parameters.AddWithValue("@price", tour.Price);
                        cmd.Parameters.AddWithValue("@startDate", tour.StartDate);
                        cmd.Parameters.AddWithValue("@endDate", tour.EndDate);
                        cmd.Parameters.AddWithValue("@destination", tour.Destination);
                        cmd.Parameters.AddWithValue("@availableSeats", tour.AvailableSeats);
                        cmd.Parameters.AddWithValue("@status", tour.Status);
                        cmd.Parameters.AddWithValue("@transportid", tour.TransportId ?? (object)DBNull.Value);

                        return await cmd.ExecuteNonQueryAsync() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Логгирование исключения
                Console.WriteLine($"Ошибка при добавлении тура: {ex.Message}");
                throw;
            }
        }


        public async Task<bool> UpdateTourAsync(TourDTO tour)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"UPDATE Tours 
                         SET Name = @Name, Description = @Description, 
                         Price = @Price, StartDate = @StartDate, 
                         EndDate = @EndDate, Destination = @Destination,
                         AvailableSeats = @AvailableSeats, Status = @Status,
                         TransportId = @TransportId
                         WHERE TourId = @TourId";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@TourId", tour.TourId);
                    cmd.Parameters.AddWithValue("@Name", tour.Name);
                    cmd.Parameters.AddWithValue("@Description", tour.Description);
                    cmd.Parameters.AddWithValue("@Price", tour.Price);
                    cmd.Parameters.AddWithValue("@StartDate", tour.StartDate);
                    cmd.Parameters.AddWithValue("@EndDate", tour.EndDate);
                    cmd.Parameters.AddWithValue("@Destination", tour.Destination);
                    cmd.Parameters.AddWithValue("@AvailableSeats", tour.AvailableSeats);
                    cmd.Parameters.AddWithValue("@Status", tour.Status);

                    // Если TransportId может быть null, передаем DBNull.Value
                    cmd.Parameters.AddWithValue("@TransportId", tour.TransportId ?? (object)DBNull.Value);

                    return await cmd.ExecuteNonQueryAsync() > 0;
                }

            }
        }

        public async Task<bool> DeleteTourAsync(int tourId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "DELETE FROM Tours WHERE TourId = @TourId";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@TourId", tourId);
                    return await cmd.ExecuteNonQueryAsync() > 0;
                }
            }
        }


        public async Task<List<TourWithImageDTO>> GetToursAsync()
        {
            var tours = new List<TourWithImageDTO>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
            SELECT t.TourId, t.Name, t.Description, t.Price, t.StartDate, t.EndDate, 
                   t.Destination, t.AvailableSeats, t.Status, i.ImagePath, t.TransportId
            FROM Tours t
            LEFT JOIN Images i ON t.TourId = i.TourId";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var tourDTO = new TourWithImageDTO(
                                new tours
                                {
                                    tourid = reader.GetInt32(0),
                                    name = reader.GetString(1),
                                    description = reader.GetString(2),
                                    price = reader.GetDecimal(3),
                                    startdate = reader.GetDateTime(4),
                                    enddate = reader.GetDateTime(5),
                                    destination = reader.GetString(6),
                                    availableseats = reader.GetInt32(7),
                                    status = reader.GetString(8),
                                    transportid = reader.IsDBNull(10) ? (int?)null : reader.GetInt32(10) // Добавляем TransportId
                                },
                                reader.IsDBNull(9) ? null : reader.GetString(9)
                            );

                            tours.Add(tourDTO);
                        }
                    }
                }
            }

            return tours;
        }

        public async Task<TourWithImageDTO> GetTourDetailsAsync(int tourId)
        {
            tours tour = null;
            string imagePath = null;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"
                SELECT t.TourId, t.Name, t.Description, t.Price, t.StartDate, t.EndDate, 
                       t.Destination, t.AvailableSeats, t.Status, i.ImagePath
                FROM Tours t
                LEFT JOIN Images i ON t.TourId = i.TourId
                WHERE t.TourId = @TourId";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TourId", tourId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            tour = new tours
                            {
                                tourid = reader.GetInt32(0),
                                name = reader.GetString(1),
                                description = reader.GetString(2),
                                price = reader.GetDecimal(3),
                                startdate = reader.GetDateTime(4),
                                enddate = reader.GetDateTime(5),
                                destination = reader.GetString(6),
                                availableseats = reader.GetInt32(7),
                                status = reader.GetString(8)
                            };

                            imagePath = reader.IsDBNull(9) ? null : reader.GetString(9);
                        }
                    }
                }
            }

            return tour != null ? new TourWithImageDTO(tour, imagePath) : null;
        }

        public async Task<bool> BookTourAsync(int tourId, int userId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Проверка доступности мест
                        var checkAvailabilityQuery = @"
                        SELECT AvailableSeats 
                        FROM Tours 
                        WHERE TourId = @TourId AND AvailableSeats > 0";

                        using (var command = new NpgsqlCommand(checkAvailabilityQuery, connection))
                        {
                            command.Parameters.AddWithValue("@TourId", tourId);
                            var availableSeats = await command.ExecuteScalarAsync();

                            if (availableSeats == null)
                            {
                                return false; // Нет доступных мест или тур не найден
                            }
                        }

                        // Уменьшение количества доступных мест
                        var updateSeatsQuery = @"
                        UPDATE Tours 
                        SET AvailableSeats = AvailableSeats - 1 
                        WHERE TourId = @TourId";

                        using (var command = new NpgsqlCommand(updateSeatsQuery, connection))
                        {
                            command.Parameters.AddWithValue("@TourId", tourId);
                            await command.ExecuteNonQueryAsync();
                        }

                        // Создание бронирования
                        var createBookingQuery = @"
                        INSERT INTO Bookings (UserId, TourId, BookingDate, NumberOfSeats) 
                        VALUES (@UserId, @TourId, @BookingDate, 1)";

                        using (var command = new NpgsqlCommand(createBookingQuery, connection))
                        {
                            command.Parameters.AddWithValue("@UserId", userId);
                            command.Parameters.AddWithValue("@TourId", tourId);
                            command.Parameters.AddWithValue("@BookingDate", DateTime.UtcNow);
                            await command.ExecuteNonQueryAsync();
                        }

                        await transaction.CommitAsync();
                        return true;
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }
                }
            }
        }
    }
}
