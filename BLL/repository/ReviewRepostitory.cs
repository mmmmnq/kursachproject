using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DTO;

namespace BLL.Repositories
{
    public class ReviewRepository
    {
        private readonly string _connectionString;

        public ReviewRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<ReviewDTO>> GetReviewsByTourIdAsync(int tourId)
        {
            var reviews = new List<ReviewDTO>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"
                    SELECT r.reviewid, r.userid, r.tourid, r.rating, r.comment, r.reviewdate, u.username
                    FROM public.reviews r
                    INNER JOIN public.users u ON r.userid = u.userid
                    WHERE r.tourid = @TourId
                    ORDER BY r.reviewdate DESC";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TourId", tourId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            reviews.Add(new ReviewDTO
                            {
                                ReviewId = reader.GetInt32(0),
                                UserId = reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1),
                                TourId = reader.GetInt32(2),
                                Rating = reader.GetInt32(3),
                                Comment = reader.IsDBNull(4) ? null : reader.GetString(4),
                                ReviewDate = reader.GetDateTime(5),
                                UserName = reader.IsDBNull(6) ? null : reader.GetString(6)
                            });
                        }
                    }
                }
            }

            return reviews;
        }

        public async Task<List<ReviewDTO>> GetAllReviewsAsync()
        {
            var reviews = new List<ReviewDTO>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Изменённый запрос с JOIN к таблице users
                var query = @"
            SELECT r.reviewid, r.userid, r.tourid, r.rating, r.comment, r.reviewdate, u.username
            FROM reviews r
            LEFT JOIN users u ON r.userid = u.userid
            ORDER BY r.reviewdate DESC";

                using (var command = new NpgsqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        reviews.Add(new ReviewDTO
                        {
                            ReviewId = reader.GetInt32(0),
                            UserId = reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1),
                            TourId = reader.GetInt32(2),
                            Rating = reader.GetInt32(3),
                            Comment = reader.IsDBNull(4) ? null : reader.GetString(4),
                            ReviewDate = reader.GetDateTime(5),
                            UserName = reader.IsDBNull(6) ? null : reader.GetString(6)
                        });
                    }
                }
            }

            return reviews;
        }

        public async Task<bool> HasUserPurchasedTourAsync(int userId, int tourId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                const string query = @"
            SELECT COUNT(1) 
            FROM public.bookings b
            WHERE b.userid = @UserId 
            AND b.tourid = @TourId
            AND b.bookingdate <= CURRENT_TIMESTAMP"; // Проверяем что бронирование уже состоялось

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@TourId", tourId);

                    var count = Convert.ToInt32(await command.ExecuteScalarAsync());
                    return count > 0;
                }
            }
        }

        public async Task AddReviewAsync(ReviewDTO review)
        {
            if (!CurrentUser.IsUserLoggedIn())
            {
                throw new UnauthorizedAccessException("Необходимо авторизоваться для добавления отзыва.");
            }

            // Проверяем наличие бронирования
            if (!await HasUserPurchasedTourAsync(CurrentUser.UserId, review.TourId))
            {
                throw new UnauthorizedAccessException("Отзыв могут оставлять только пользователи, которые забронировали данный тур.");
            }

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Проверка существующего отзыва
                const string checkExisting = @"
            SELECT COUNT(1) FROM public.reviews 
            WHERE userid = @UserId AND tourid = @TourId";

                using (var command = new NpgsqlCommand(checkExisting, connection))
                {
                    command.Parameters.AddWithValue("@UserId", CurrentUser.UserId);
                    command.Parameters.AddWithValue("@TourId", review.TourId);
                    var hasReview = Convert.ToInt32(await command.ExecuteScalarAsync()) > 0;
                    if (hasReview)
                    {
                        throw new InvalidOperationException("Вы уже оставляли отзыв для этого тура.");
                    }
                }

                // Добавление отзыва (используем старый рабочий запрос)
                const string insertQuery = @"
            INSERT INTO public.reviews 
            (userid, tourid, rating, comment, reviewdate) 
            VALUES (@UserId, @TourId, @Rating, @Comment, @ReviewDate)";

                using (var command = new NpgsqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@UserId", CurrentUser.UserId);
                    command.Parameters.AddWithValue("@TourId", review.TourId);
                    command.Parameters.AddWithValue("@Rating", review.Rating);
                    command.Parameters.AddWithValue("@Comment", review.Comment ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ReviewDate", DateTime.UtcNow);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task UpdateReviewAsync(ReviewDTO review)
        {
            if (!CurrentUser.IsUserLoggedIn())
            {
                throw new UnauthorizedAccessException("Необходимо авторизоваться для изменения отзыва.");
            }

            // Проверяем наличие бронирования
            if (!await HasUserPurchasedTourAsync(CurrentUser.UserId, review.TourId))
            {
                throw new UnauthorizedAccessException("Изменять отзыв могут только пользователи, которые забронировали данный тур.");
            }

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                const string updateQuery = @"
            UPDATE public.reviews 
            SET rating = @Rating, comment = @Comment 
            WHERE reviewid = @ReviewId AND userid = @UserId";

                using (var command = new NpgsqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@ReviewId", review.ReviewId);
                    command.Parameters.AddWithValue("@UserId", CurrentUser.UserId);
                    command.Parameters.AddWithValue("@Rating", review.Rating);
                    command.Parameters.AddWithValue("@Comment", review.Comment ?? (object)DBNull.Value);

                    int affectedRows = await command.ExecuteNonQueryAsync();
                    if (affectedRows == 0)
                    {
                        throw new Exception("Отзыв не найден или у вас нет прав на его изменение.");
                    }
                }
            }
        }

        public async Task DeleteReviewAsync(int reviewId)
        {
            const string query = "DELETE FROM public.reviews WHERE reviewid = @ReviewId";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ReviewId", reviewId);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
