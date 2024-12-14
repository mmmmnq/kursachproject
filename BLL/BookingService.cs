using Npgsql;
using System;

public class BookingService
{
    private readonly string _connectionString;

    public BookingService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public bool BookTour(int userId, int tourId, int numberOfSeats)
    {
        try
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    // Получаем стоимость тура и транспорта
                    string priceQuery = "SELECT t.price, tr.price, tr.capacity, t.availableseats FROM tours t " +
                                        "JOIN transport tr ON t.transportid = tr.transportid WHERE t.tourid = @tourId";
                    decimal tourPrice, transportPrice;
                    int transportCapacity, availableSeats;

                    using (var cmd = new NpgsqlCommand(priceQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("tourId", tourId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                                throw new Exception("Тур не найден!");

                            tourPrice = reader.GetDecimal(0);
                            transportPrice = reader.GetDecimal(1);
                            transportCapacity = reader.GetInt32(2);
                            availableSeats = reader.GetInt32(3);
                        }
                    }

                    if (transportCapacity < numberOfSeats || availableSeats < numberOfSeats)
                        throw new Exception("Недостаточно мест для бронирования!");

                    // Вычисляем итоговую сумму
                    decimal totalPrice = (tourPrice + transportPrice) * numberOfSeats;

                    // Вставляем данные в таблицу bookings
                    string insertBookingQuery = "INSERT INTO bookings (userid, tourid, numberofseats, totalprice) " +
                                                "VALUES (@userId, @tourId, @numberOfSeats, @totalPrice)";
                    using (var cmd = new NpgsqlCommand(insertBookingQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("userId", userId);
                        cmd.Parameters.AddWithValue("tourId", tourId);
                        cmd.Parameters.AddWithValue("numberOfSeats", numberOfSeats);
                        cmd.Parameters.AddWithValue("totalPrice", totalPrice);

                        cmd.ExecuteNonQuery();
                    }

                    // Обновляем количество мест в транспорте
                    string updateTransportQuery = "UPDATE transport SET capacity = capacity - @numberOfSeats WHERE transportid = (SELECT transportid FROM tours WHERE tourid = @tourId)";
                    using (var cmd = new NpgsqlCommand(updateTransportQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("numberOfSeats", numberOfSeats);
                        cmd.Parameters.AddWithValue("tourId", tourId);

                        cmd.ExecuteNonQuery();
                    }

                    // Обновляем доступные места в туре
                    string updateTourQuery = "UPDATE tours SET availableseats = availableseats - @numberOfSeats WHERE tourid = @tourId";
                    using (var cmd = new NpgsqlCommand(updateTourQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("numberOfSeats", numberOfSeats);
                        cmd.Parameters.AddWithValue("tourId", tourId);

                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    return true;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка бронирования: {ex.Message}");
            return false;
        }
    }
}
