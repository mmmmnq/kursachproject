using System.Collections.ObjectModel;
using System.Windows.Input;
using kursovaya.Helpers;
using kursovaya.Views;

namespace kursovaya.ViewModels
{
    public class TourViewModel
    {
        private readonly ITourService _tourService;
        public ICommand BookCommand { get; }
        public ObservableCollection<TourWithImage> Tours { get; set; }
        public ICommand DetailsCommand { get; }

        public TourViewModel()
        {
            _tourService = new TourService("Host=localhost;Port=5432;Username=postgres;Password=123;Database=TravelAgency");
            Tours = new ObservableCollection<TourWithImage>();
            DetailsCommand = new RelayCommand<TourWithImage>(ShowTourDetails);
            BookCommand = new RelayCommand<TourWithImage>(OpenBookingWindow);


            LoadTours();
        }

        private async void LoadTours()
        {
            var tours = await _tourService.GetToursAsync();
            foreach (var tour in tours)
            {
                Tours.Add(tour);
            }
        }

        private async void ShowTourDetails(TourWithImage selectedTour)
        {
            if (selectedTour == null) return;

            // Загрузка подробной информации о туре
            var detailedTour = await _tourService.GetTourDetailsAsync(selectedTour.TourId);

            // Открываем окно и передаем данные
            var detailsWindow = new Views.TourDetailsWindow
            {
                DataContext = detailedTour
            };

            detailsWindow.ShowDialog();
        }
        private void OpenBookingWindow(object parameter)
        {
            var bookingWindow = new BookingWindow();
            bookingWindow.Show();
        }




    }
}
