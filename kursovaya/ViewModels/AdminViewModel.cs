using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using kursovaya.Views;
using kursovaya.Helpers;
using Interfaces.Services;
using BLL.Services;
using BLL.Repositories;
using DTO;
namespace kursovaya.ViewModels
{
    public class AdminViewModel : LoginViewModel
    {
        public ICommand ManageUsersCommand { get; }
        public ICommand ManageToursCommand { get; }
        public ICommand ManageTransportCommand { get; }
        public ICommand ManageBookingsCommand { get; }
        public ICommand ViewReportsCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand ManageImagesCommand { get; }

        public ICommand ViewReviewCommand { get; }
        public AdminViewModel()
        {
            // Используем лямбда-выражения для передачи параметра
            ManageUsersCommand = new RelayCommand(param => ExecuteManageUsers());
            ManageToursCommand = new RelayCommand(param => ExecuteManageTours());
            ManageTransportCommand = new RelayCommand(param => ExecuteManageTransport());
            ManageBookingsCommand = new RelayCommand(param => ExecuteManageBookings());
            ViewReportsCommand = new RelayCommand(param => ExecuteViewReports());
            LogoutCommand = new RelayCommand(param => ExecuteLogout());
            ManageImagesCommand = new RelayCommand(param => ExecuteManageImages());
            ViewReviewCommand = new RelayCommand(param => ExecuteManageReviews());
        }


        private void ExecuteManageReviews()
        {
            var managReviews = new Review();
            managReviews.Show();

        }
        private void ExecuteManageUsers()
        {
            string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=123;Database=TravelAgency";
            IUserService userService = new UserService(connectionString); // Создаем экземпляр UserService

            var manageUsersWindow = new ManageUsersWindow(userService); // Передаем его в ManageUsersWindow
            manageUsersWindow.ShowDialog();
        }

        private void ExecuteManageTours()
        {
            string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=123;Database=TravelAgency"; 
            ITourService tourService = new TourService(connectionString);

            var manageTourWindow = new ManageTourWindow(tourService);
            manageTourWindow.ShowDialog();
        }

        private void ExecuteManageTransport()
        {
            var manageTransportWindow = new ManageTransportWindow();
            manageTransportWindow.ShowDialog();
        }

        private void ExecuteManageBookings()
        {
            var manageBookingsWindow = new ManageBookingsWindow();
            manageBookingsWindow.ShowDialog();
        }

        private async void ExecuteViewReports()
        {
            var bookingRepository = new BookingRepository("Host=localhost;Port=5432;Username=postgres;Password=123;Database=TravelAgency");
            var tourRepository = new TourRepository("Host=localhost;Port=5432;Username=postgres;Password=123;Database=TravelAgency");

            // Получение данных
            var bookings = await bookingRepository.GetAllBookingsAsync();
            var toursWithImages = await tourRepository.GetToursAsync();

            // Преобразование TourWithImageDTO в TourDTO
            var tours = toursWithImages.Select(t => new TourDTO
            {
                TourId = t.TourId,
                Name = t.Name,
                Description = t.Description,
                Price = t.Price,
                StartDate = t.StartDate,
                EndDate = t.EndDate,
                Destination = t.Destination,
                AvailableSeats = t.AvailableSeats,
                Status = t.Status,
                
            });

            // Создание и показ окна
            var reportsWindow = new ReportsWindow(bookings, tours);
            reportsWindow.ShowDialog();
        }



        private void ExecuteLogout()
        {
            Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is AdminForm)?.Close();
        }
        private void ExecuteManageImages()
        {
            var manageImagesWindow = new ManageImagesWindow();
            manageImagesWindow.Show(); 
        }
    }
}