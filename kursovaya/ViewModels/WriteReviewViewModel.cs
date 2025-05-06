using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using BLL.Services;
using Interfaces.Services;
using kursovaya.Helpers;
using System.Windows;
using BLL;

namespace kursovaya.ViewModels
{
    public class WriteReviewViewModel : INotifyPropertyChanged
    {
        private readonly IReviewService _reviewService;
        private readonly int _tourId;
        private readonly Window _window;

        private string _comment;
        private int _selectedRating;
        public List<int> RatingOptions { get; } = new List<int> { 1, 2, 3, 4, 5 };

        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value;
                OnPropertyChanged();
            }
        }

        public int SelectedRating
        {
            get => _selectedRating;
            set
            {
                _selectedRating = value;
                OnPropertyChanged();
            }
        }

        public ICommand SubmitReviewCommand { get; }
        public ICommand CancelCommand { get; }

        public WriteReviewViewModel(int tourId, Window window)
        {
            _tourId = tourId;
            _window = window;
            _reviewService = new ReviewService("Host=localhost;Port=5432;Username=postgres;Password=123;Database=TravelAgency");

            SelectedRating = 5; // Значение по умолчанию
            SubmitReviewCommand = new RelayCommand(async _ =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(Comment))
                    {
                        MessageBox.Show("Пожалуйста, напишите комментарий.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Проверяем, что пользователь авторизован
                    if (!CurrentUser.IsAuthenticated)
                    {
                        MessageBox.Show("Вы не авторизованы. Пожалуйста, войдите в систему.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    await _reviewService.AddReviewAsync(new DTO.ReviewDTO
                    {
                        TourId = _tourId,
                        UserId = CurrentUser.UserId, // Получаем ID текущего пользователя
                        Rating = SelectedRating,
                        Comment = Comment,
                        ReviewDate = DateTime.Now
                    });

                    MessageBox.Show("Отзыв успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    _window.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении отзыва: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });

            CancelCommand = new RelayCommand(_ => _window.Close());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}