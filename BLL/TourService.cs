using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Npgsql;

public class TourService : ITourService
{
    private readonly string _connectionString;

    public TourService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<TourWithImage>> GetToursAsync()
    {
        var tours = new List<TourWithImage>();

        using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            string query = @"
                SELECT t.TourId, t.Name, t.Description, t.Price, t.StartDate, t.EndDate, 
                       t.Destination, t.AvailableSeats, t.Status, i.ImagePath
                FROM Tours t
                LEFT JOIN Images i ON t.TourId = i.TourId";

            using (var cmd = new NpgsqlCommand(query, connection))
            {
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        tours.Add(new TourWithImage
                        {
                            TourId = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            Price = reader.GetDecimal(3),
                            StartDate = reader.GetDateTime(4),
                            EndDate = reader.GetDateTime(5),
                            Destination = reader.GetString(6),
                            AvailableSeats = reader.GetInt32(7),
                            Status = reader.GetString(8),
                            ImagePath = reader.IsDBNull(9) ? "default.jpg" : reader.GetString(9)
                        });
                    }
                }
            }
        }

        return tours;
    }

    public async Task<TourWithImage> GetTourDetailsAsync(int tourId)
    {
        TourWithImage tour = null;

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
                    if (await reader.ReadAsync()) // Ожидается одна строка
                    {
                        tour = new TourWithImage
                        {
                            TourId = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.GetString(2),
                            Price = reader.GetDecimal(3),
                            StartDate = reader.GetDateTime(4),
                            EndDate = reader.GetDateTime(5),
                            Destination = reader.GetString(6),
                            AvailableSeats = reader.GetInt32(7),
                            Status = reader.GetString(8),
                            ImagePath = reader.IsDBNull(9) ? "default.jpg" : reader.GetString(9)
                        };
                    }
                }
            }
        }

        return tour;
    }


    public async Task<bool> BookTourAsync(int tourId, int userId)
    {
        // Логика бронирования тура
        throw new NotImplementedException();
    }
}
