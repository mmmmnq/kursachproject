using BLL.Services;
using DTO;
using kursovaya.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using kursovaya.Helpers;

namespace kursovaya.ViewModel
{
    public class ManageReviewsViewModel : BaseViewModel
    {
        private readonly ReviewService _reviewService;
        private ObservableCollection<ReviewDTO> _reviews;
        private ReviewDTO _selectedReview;

        public ObservableCollection<ReviewDTO> Reviews
        {
            get => _reviews;
            set
            {
                _reviews = value;
                OnPropertyChanged(nameof(Reviews));
            }
        }

        public ReviewDTO SelectedReview
        {
            get => _selectedReview;
            set
            {
                _selectedReview = value;
                OnPropertyChanged(nameof(SelectedReview));
            }
        }

        public ICommand DeleteReviewCommand { get; }

        public ManageReviewsViewModel()
        {
            _reviewService = new ReviewService("Host=localhost;Port=5432;Username=postgres;Password=123;Database=TravelAgency");
            Reviews = new ObservableCollection<ReviewDTO>();

            LoadReviewsAsync();

            DeleteReviewCommand = new RelayCommand(async _ => await ExecuteDeleteReviewAsync());
        }

        private async Task LoadReviewsAsync()
        {
            try
            {
                var reviews = await _reviewService.GetAllReviewsAsync();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Reviews.Clear();
                    foreach (var review in reviews)
                    {
                        Reviews.Add(review);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
            }
        }

        private async Task ExecuteDeleteReviewAsync()
        {
            if (SelectedReview != null)
            {
                try
                {
                    await _reviewService.DeleteReviewAsync(SelectedReview.ReviewId);
                    Reviews.Remove(SelectedReview);
                    SelectedReview = null; // Очищаем выбор после удаления
                    MessageBox.Show("Отзыв успешно удален!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении отзыва: {ex.Message}");
                }
            }
        }

        private bool CanExecuteDeleteReview(object parameter)
        {
            return SelectedReview != null;
        }
    }
}
