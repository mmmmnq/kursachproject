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
    public class ManageBookingsViewModel : BaseViewModel
    {
        private readonly BookingService _bookingService;
        private ObservableCollection<BookingDTO> _bookings;
        private BookingDTO _selectedBooking;

        public ObservableCollection<BookingDTO> Bookings
        {
            get => _bookings;
            set
            {
                _bookings = value;
                OnPropertyChanged(nameof(Bookings));
            }
        }

        public BookingDTO SelectedBooking
        {
            get => _selectedBooking;
            set
            {
                _selectedBooking = value;
                OnPropertyChanged(nameof(SelectedBooking));
            }
        }

        public ICommand DeleteBookingCommand { get; }

        public ManageBookingsViewModel()
        {
            _bookingService = new BookingService("Host=localhost;Port=5432;Username=postgres;Password=123;Database=TravelAgency");
            Bookings = new ObservableCollection<BookingDTO>();
            DeleteBookingCommand = new RelayCommand(async _ => await ExecuteDeleteBookingAsync());
            LoadBookingsAsync(); 
        }

        private async Task LoadBookingsAsync()
        {
            try
            {
                var bookings = await _bookingService.GetAllBookingsAsync();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Bookings.Clear();
                    foreach (var booking in bookings)
                    {
                        Bookings.Add(booking);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке бронирований: {ex.Message}");
            }
        }

        private async Task ExecuteDeleteBookingAsync()
        {
            if (SelectedBooking != null)
            {
                try
                {
                    await _bookingService.DeleteBookingAsync(SelectedBooking.BookingId);
                    Bookings.Remove(SelectedBooking);
                    SelectedBooking = null;
                    MessageBox.Show("Бронирование успешно удалено!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении бронирования: {ex.Message}");
                }
            }
        }

        private bool CanExecuteDeleteBooking(object parameter)
        {
            return SelectedBooking != null;
        }
    }
}