using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;
using DTO;
using DomainModel;

namespace BLL.Repositories
{
    public class BookingRepository
    {
        private readonly string _connectionString;

        public BookingRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<(string TourName, string Destination)> GetTourDetailsAsync(int tourId)
        {
            const string query = @"
                SELECT Name AS TourName, Destination 
                FROM Tours 
                WHERE TourId = @TourId";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TourId", tourId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            string tourName = reader.GetString(reader.GetOrdinal("TourName"));
                            string destination = reader.GetString(reader.GetOrdinal("Destination"));
                            return (tourName, destination);
                        }
                        return ("Неизвестный тур", "Неизвестно");
                    }
                }
            }
        }

        public async Task<string> GetUserNameAsync(int userId)
        {
            const string query = @"
                SELECT FirstName, LastName 
                FROM Users 
                WHERE UserId = @UserId";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            string firstName = reader.GetString(reader.GetOrdinal("FirstName"));
                            string lastName = reader.GetString(reader.GetOrdinal("LastName"));
                            return $"{firstName} {lastName}";
                        }
                        return "Неизвестный пользователь";
                    }
                }
            }
        }

        public async Task AddBookingAsync(int userId, int tourId, int numberOfSeats, decimal totalPrice)
        {
            const string insertBookingQuery = @"
                INSERT INTO Bookings (UserId, TourId, BookingDate, NumberOfSeats, TotalPrice)
                VALUES (@UserId, @TourId, CURRENT_TIMESTAMP, @NumberOfSeats, @TotalPrice)";

            const string updateTourSeatsQuery = @"
                UPDATE Tours SET AvailableSeats = AvailableSeats - @NumberOfSeats 
                WHERE TourId = @TourId AND AvailableSeats >= @NumberOfSeats";

            const string updateTransportCapacityQuery = @"
                UPDATE Transport SET Capacity = Capacity - @NumberOfSeats 
                WHERE TransportId = (SELECT TransportId FROM Tours WHERE TourId = @TourId) AND Capacity >= @NumberOfSeats";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var command = new NpgsqlCommand(updateTourSeatsQuery, connection))
                        {
                            command.Parameters.AddWithValue("@TourId", tourId);
                            command.Parameters.AddWithValue("@NumberOfSeats", numberOfSeats);
                            var rowsAffected = await command.ExecuteNonQueryAsync();

                            if (rowsAffected == 0)
                                throw new Exception("Недостаточно мест в туре.");
                        }

                        using (var command = new NpgsqlCommand(updateTransportCapacityQuery, connection))
                        {
                            command.Parameters.AddWithValue("@TourId", tourId);
                            command.Parameters.AddWithValue("@NumberOfSeats", numberOfSeats);
                            var rowsAffected = await command.ExecuteNonQueryAsync();

                            if (rowsAffected == 0)
                                throw new Exception("Недостаточно мест в транспорте.");
                        }

                        using (var command = new NpgsqlCommand(insertBookingQuery, connection))
                        {
                            command.Parameters.AddWithValue("@UserId", userId);
                            command.Parameters.AddWithValue("@TourId", tourId);
                            command.Parameters.AddWithValue("@NumberOfSeats", numberOfSeats);
                            command.Parameters.AddWithValue("@TotalPrice", totalPrice);
                            await command.ExecuteNonQueryAsync();
                        }

                        await transaction.CommitAsync();
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }

        public async Task DeleteBookingAsync(int bookingId)
        {
            const string getBookingInfoQuery = @"
        SELECT TourId, NumberOfSeats 
        FROM Bookings 
        WHERE BookingId = @BookingId";

            const string deleteBookingQuery = @"
        DELETE FROM Bookings 
        WHERE BookingId = @BookingId";

            const string updateTourSeatsQuery = @"
        UPDATE Tours 
        SET AvailableSeats = AvailableSeats + @NumberOfSeats 
        WHERE TourId = @TourId";

            const string updateTransportCapacityQuery = @"
        UPDATE Transport 
        SET Capacity = Capacity + @NumberOfSeats 
        WHERE TransportId = (SELECT TransportId FROM Tours WHERE TourId = @TourId)";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int tourId = 0;
                        int numberOfSeats = 0;

                        // Получаем информацию о бронировании
                        using (var command = new NpgsqlCommand(getBookingInfoQuery, connection))
                        {
                            command.Parameters.AddWithValue("@BookingId", bookingId);
                            using (var reader = await command.ExecuteReaderAsync())
                            {
                                if (await reader.ReadAsync())
                                {
                                    tourId = reader.GetInt32(reader.GetOrdinal("TourId"));
                                    numberOfSeats = reader.GetInt32(reader.GetOrdinal("NumberOfSeats"));
                                }
                            }
                        }

                        // Возвращаем места в тур
                        using (var command = new NpgsqlCommand(updateTourSeatsQuery, connection))
                        {
                            command.Parameters.AddWithValue("@TourId", tourId);
                            command.Parameters.AddWithValue("@NumberOfSeats", numberOfSeats);
                            await command.ExecuteNonQueryAsync();
                        }

                        // Возвращаем места в транспорт
                        using (var command = new NpgsqlCommand(updateTransportCapacityQuery, connection))
                        {
                            command.Parameters.AddWithValue("@TourId", tourId);
                            command.Parameters.AddWithValue("@NumberOfSeats", numberOfSeats);
                            await command.ExecuteNonQueryAsync();
                        }

                        // Удаляем бронирование
                        using (var command = new NpgsqlCommand(deleteBookingQuery, connection))
                        {
                            command.Parameters.AddWithValue("@BookingId", bookingId);
                            await command.ExecuteNonQueryAsync();
                        }

                        await transaction.CommitAsync();
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }

        public async Task<IEnumerable<BookingDTO>> GetAllBookingsAsync()
        {
            var bookings = new List<BookingDTO>();
            const string query = @"
        SELECT b.BookingId, b.UserId, b.TourId, b.BookingDate, b.NumberOfSeats, b.TotalPrice,
               u.FirstName || ' ' || u.LastName AS UserName
        FROM Bookings b
        LEFT JOIN Users u ON b.UserId = u.UserId";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        bookings.Add(new BookingDTO
                        {
                            BookingId = reader.GetInt32(reader.GetOrdinal("BookingId")),
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            TourId = reader.GetInt32(reader.GetOrdinal("TourId")),
                            BookingDate = reader.GetDateTime(reader.GetOrdinal("BookingDate")),
                            NumberOfSeats = reader.GetInt32(reader.GetOrdinal("NumberOfSeats")),
                            TotalPrice = reader.GetDecimal(reader.GetOrdinal("TotalPrice")),
                            UserName = reader.GetString(reader.GetOrdinal("UserName"))
                        });
                    }
                }
            }
            return bookings;
        }
        public async Task<bool> HasUserBookedTourAsync(int userId, int tourId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var sql = @"
                SELECT COUNT(*) 
                FROM bookings 
                WHERE user_id = @userId 
                AND tour_id = @tourId";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("userId", userId);
                    command.Parameters.AddWithValue("tourId", tourId);

                    var result = await command.ExecuteScalarAsync();
                    return Convert.ToInt32(result) > 0;
                }
            }
        }

        public async Task<BookingDTO> GetBookingDetailsAsync(int bookingId)
        {
            const string query = @"
        SELECT b.BookingId, b.UserId, b.TourId, b.BookingDate, b.NumberOfSeats, b.TotalPrice,
               u.FirstName || ' ' || u.LastName AS UserName
        FROM Bookings b
        LEFT JOIN Users u ON b.UserId = u.UserId
        WHERE BookingId = @BookingId";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookingId", bookingId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new BookingDTO
                            {
                                BookingId = reader.GetInt32(reader.GetOrdinal("BookingId")),
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                TourId = reader.GetInt32(reader.GetOrdinal("TourId")),
                                BookingDate = reader.GetDateTime(reader.GetOrdinal("BookingDate")),
                                NumberOfSeats = reader.GetInt32(reader.GetOrdinal("NumberOfSeats")),
                                TotalPrice = reader.GetDecimal(reader.GetOrdinal("TotalPrice")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName"))
                            };
                        }
                        return null;
                    }
                }
            }
        }

        public async Task<IEnumerable<BookingDTO>> GetUserBookingsAsync(int userId)
        {
            var bookings = new List<BookingDTO>();
            const string query = @"
        SELECT b.BookingId, b.UserId, b.TourId, b.BookingDate, b.NumberOfSeats, b.TotalPrice,
               u.FirstName || ' ' || u.LastName AS UserName
        FROM Bookings b
        LEFT JOIN Users u ON b.UserId = u.UserId
        WHERE b.UserId = @UserId";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            bookings.Add(new BookingDTO
                            {
                                BookingId = reader.GetInt32(reader.GetOrdinal("BookingId")),
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                TourId = reader.GetInt32(reader.GetOrdinal("TourId")),
                                BookingDate = reader.GetDateTime(reader.GetOrdinal("BookingDate")),
                                NumberOfSeats = reader.GetInt32(reader.GetOrdinal("NumberOfSeats")),
                                TotalPrice = reader.GetDecimal(reader.GetOrdinal("TotalPrice")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName"))
                            });
                        }
                    }
                }
            }

            return bookings;
        }
    }
}
