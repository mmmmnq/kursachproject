    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;
    using BLL;
    using DAL;
    using DAL.DTO;
    using kursovaya.Helpers;
    using kursovaya.Views;
    using Interfaces.Services;
    using BLL.Services;
    using System.Windows;

    namespace kursovaya.ViewModels
    {
        public class TourDetailsViewModel : INotifyPropertyChanged
        {
            private readonly ITourService _tourService;
            private readonly IReviewService _reviewService;

            private int _tourId;
            private string _name;
            private string _description;
            private DateTime _startDate;
            private DateTime _endDate;
            private string _destination;
            private int _availableSeats;
            private string _status;
            private decimal _price;
            private string _imagePath;

            private ObservableCollection<reviews> _reviews;

            public int TourId
            {
                get => _tourId;
                set { _tourId = value; OnPropertyChanged(); }
            }

            public string Name
            {
                get => _name;
                set { _name = value; OnPropertyChanged(); }
            }

            public string Description
            {
                get => _description;
                set { _description = value; OnPropertyChanged(); }
            }

            public DateTime StartDate
            {
                get => _startDate;
                set { _startDate = value; OnPropertyChanged(); }
            }

            public DateTime EndDate
            {
                get => _endDate;
                set { _endDate = value; OnPropertyChanged(); }
            }

            public string Destination
            {
                get => _destination;
                set { _destination = value; OnPropertyChanged(); }
            }

            public int AvailableSeats
            {
                get => _availableSeats;
                set { _availableSeats = value; OnPropertyChanged(); }
            }

            public string Status
            {
                get => _status;
                set { _status = value; OnPropertyChanged(); }
            }

            public decimal Price
            {
                get => _price;
                set { _price = value; OnPropertyChanged(); }
            }

            public string ImagePath
            {
                get => _imagePath;
                set { _imagePath = value; OnPropertyChanged(); }
            }

            public ObservableCollection<reviews> Reviews
            {
                get => _reviews;
                set { _reviews = value; OnPropertyChanged(); }
            }

            public ICommand WriteReviewCommand { get; }

            public TourDetailsViewModel(int tourId)
            {
                TourId = tourId;
                _tourService = new TourService("Host=localhost;Port=5432;Username=postgres;Password=123;Database=TravelAgency");
                _reviewService = new ReviewService("Host=localhost;Port=5432;Username=postgres;Password=123;Database=TravelAgency");

                Reviews = new ObservableCollection<reviews>();

                WriteReviewCommand = new RelayCommand(_ => OpenWriteReviewWindow());

                LoadTourDetails();
                LoadReviews();
            }

            private async void LoadTourDetails()
            {
                var tour = await _tourService.GetTourDetailsAsync(TourId);
                if (tour != null)
                {
                    Name = tour.Name;
                    Description = tour.Description;
                    StartDate = tour.StartDate;
                    EndDate = tour.EndDate;
                    Destination = tour.Destination;
                    AvailableSeats = tour.AvailableSeats;
                    Status = tour.Status;
                    Price = tour.Price;
                    ImagePath = tour.ImagePath;
                }
            }

            private async void LoadReviews()
            {
            var reviewDtos = await _reviewService.GetReviewsByTourIdAsync(TourId);
            Reviews.Clear();
            foreach (var reviewDto in reviewDtos)
            {
                Reviews.Add(new reviews
                {
                    reviewid = reviewDto.ReviewId,
                    userid = reviewDto.UserId,
                    tourid = reviewDto.TourId,
                    rating = reviewDto.Rating,
                    comment = reviewDto.Comment,
                    UserName = reviewDto.UserId == CurrentUser.UserId ? CurrentUser.Username : reviewDto.UserName, // Используем имя текущего пользователя, если оно совпадает
                    reviewdate = reviewDto.ReviewDate
                });
            }
        }



            private void OpenWriteReviewWindow()
            {
                var writeReviewWindow = new WriteReviewWindow(TourId);
                writeReviewWindow.ShowDialog();

                // После закрытия окна перезагружаем отзывы
                LoadReviews();
            }



            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }