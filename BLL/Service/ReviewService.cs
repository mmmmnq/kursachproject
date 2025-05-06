using BLL.Repositories;
using DAL;
using DTO;
using Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ReviewRepository _reviewRepository;

        public ReviewService(string connectionString)
        {
            _reviewRepository = new ReviewRepository(connectionString);
        }

        public async Task<List<ReviewDTO>> GetReviewsByTourIdAsync(int tourId)
        {
            // Получаем отзывы через репозиторий
            return await _reviewRepository.GetReviewsByTourIdAsync(tourId);
        }

        public async Task AddReviewAsync(ReviewDTO review)
        {
            // Валидация входных данных
            ValidateReview(review);

            // Добавляем отзыв через репозиторий
            await _reviewRepository.AddReviewAsync(review);
        }

        public async Task<List<ReviewDTO>> GetAllReviewsAsync()
        {
            return await _reviewRepository.GetAllReviewsAsync();
        }

        public async Task UpdateReviewAsync(ReviewDTO review)
        {
            // Валидация входных данных
            ValidateReview(review);

            // Обновляем отзыв через репозиторий
            await _reviewRepository.UpdateReviewAsync(review);
        }

        public async Task DeleteReviewAsync(int reviewId)
        {
            // Удаляем отзыв через репозиторий
            await _reviewRepository.DeleteReviewAsync(reviewId);
        }

        private void ValidateReview(ReviewDTO review)
        {
            if (review == null)
                throw new ArgumentNullException(nameof(review), "Отзыв не может быть пустым.");

            if (review.TourId <= 0)
                throw new ArgumentException("Некорректный идентификатор тура.");

            if (review.Rating < 1 || review.Rating > 5)
                throw new ArgumentException("Рейтинг должен быть от 1 до 5.");

            if (!string.IsNullOrWhiteSpace(review.Comment) && review.Comment.Length > 500)
                throw new ArgumentException("Комментарий слишком длинный. Максимальная длина - 500 символов.");
        }
    }
}
