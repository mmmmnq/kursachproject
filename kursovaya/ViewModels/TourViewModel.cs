using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using BLL;
using DAL;
using kursovaya.Helpers;
using kursovaya.Views;
using Interfaces.Services;
using BLL.Services;

namespace kursovaya.ViewModels
{
    public class TourViewModel : INotifyPropertyChanged
    {
        private readonly ITourService _tourService;
        private readonly TransportService _transportService;

        private ObservableCollection<TourWithImage> _tours;
        public ObservableCollection<TourWithImage> Tours
        {
            get => _tours;
            set
            {
                _tours = value;
                OnPropertyChanged();
            }
        }

        public ICommand DetailsCommand { get; }
        public ICommand BookCommand { get; }
        public ICommand ReviewsCommand { get; }

        public TourViewModel()
        {
            _tourService = new TourService("Host=localhost;Port=5432;Username=postgres;Password=123;Database=TravelAgency");
            _transportService = new TransportService("Host=localhost;Port=5432;Username=postgres;Password=123;Database=TravelAgency");

            Tours = new ObservableCollection<TourWithImage>();

            DetailsCommand = new RelayCommand<TourWithImage>(ShowTourDetails);
            BookCommand = new RelayCommand<TourWithImage>(OpenBookingWindow);
            //ReviewsCommand = new RelayCommand<TourWithImage>(OpenReviewsWindow);

            LoadTours();
        }

        private async void LoadTours()
        {
            var tours = await _tourService.GetToursAsync();
            Tours.Clear(); // Очищаем перед загрузкой новых туров

            foreach (var tour in tours)
            {
                var transportPrice = await _transportService.GetTransportPriceAsync(tour.TourId);
                Tours.Add(new TourWithImage
                {
                    TourId = tour.TourId,
                    Name = tour.Name,
                    Description = tour.Description,
                    Price = tour.Price,
                    StartDate = tour.StartDate,
                    EndDate = tour.EndDate,
                    Destination = tour.Destination,
                    AvailableSeats = tour.AvailableSeats,
                    Status = tour.Status,
                    ImagePath = tour.ImagePath,
                    TransportPrice = transportPrice
                });
            }
        }

        //private void OpenReviewsWindow(TourWithImage selectedTour)
        //{
        //    if (selectedTour == null) return;
        //    var reviewsWindow = new ViewReviewsWindow(selectedTour.TourId);
        //    reviewsWindow.ShowDialog();
        //}

        private void ShowTourDetails(TourWithImage selectedTour)
        {
            if (selectedTour == null) return;
            var detailsWindow = new TourDetailsWindow(selectedTour.TourId);
            detailsWindow.ShowDialog();
        }

        private void OpenBookingWindow(TourWithImage selectedTour)
        {
            if (selectedTour == null) return;
            int userId = CurrentUser.UserId;
            var bookingWindow = new BookingWindow(
                userId,
                selectedTour.TourId,
                selectedTour.Price,
                selectedTour.TransportPrice,
                "Host=localhost;Port=5432;Username=postgres;Password=123;Database=TravelAgency"
            );
            bookingWindow.Show();
        }

        // Реализация INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}