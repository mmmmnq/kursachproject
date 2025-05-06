using kursovaya.Helpers;
using kursovaya.ViewModels;
using System.Threading.Tasks;
using System.Windows;
using System;
using System.IO;
using Interfaces.Services;
using BLL.Services;

public class BookingViewModel : LoginViewModel
{

    public event EventHandler RequestClose;
    private readonly EmailService _emailService;
    private readonly BookingService _bookingService;
    private readonly TransportService _transportService;
    private readonly IPdfTicketService _pdfTicketService;

    private string _userName;
    private readonly int _userId;
    private string _tourName;
    private string _destination;
    private readonly int _tourId;
    private readonly decimal _tourPrice;
    private decimal _transportPrice;

    public string Email { get; set; }
    public decimal TourPrice => _tourPrice;

    public decimal TransportPrice
    {
        get => _transportPrice;
        private set
        {
            if (_transportPrice != value)
            {
                _transportPrice = value;
                OnPropertyChanged(nameof(TransportPrice));
                OnPropertyChanged(nameof(TotalPrice));
            }
        }
    }

    public decimal TotalPrice => (TourPrice + TransportPrice) * NumberOfSeats;

    private int _numberOfSeats = 1;
    public int NumberOfSeats
    {
        get => _numberOfSeats;
        set
        {
            if (SetField(ref _numberOfSeats, value))
            {
                OnPropertyChanged(nameof(TotalPrice));
                // Добавляем валидацию количества мест
                ValidateNumberOfSeats();
            }
        }
    }

    public RelayCommand ConfirmCommand { get; }

    public BookingViewModel(
        int userId,
        int tourId,
        decimal tourPrice,
        decimal transportPrice,
        string connectionString,
        IPdfTicketService pdfTicketService = null)
        
    {
        _emailService = new EmailService(connectionString);
        _bookingService = new BookingService(connectionString);
        _transportService = new TransportService(connectionString);
        _pdfTicketService = pdfTicketService ?? new PdfTicketService();

        _userId = userId;
        _tourId = tourId;
        _tourPrice = tourPrice;
        _transportPrice = transportPrice;

        ConfirmCommand = new RelayCommand(async _ => await ConfirmBookingAsync());

        // Загружаем данные о пользователе и туре
        LoadUserAndTourDetailsAsync().ConfigureAwait(false);
    }

    private void ValidateNumberOfSeats()
    {
        if (_numberOfSeats <= 0)
        {
            MessageBox.Show("Количество мест должно быть больше нуля!");
        }
    }

    private async Task LoadUserAndTourDetailsAsync()
    {
        try
        {
            // Получаем имя пользователя
            _userName = await _bookingService.GetUserNameAsync(_userId);

            // Получаем название тура и место назначения
            var tourDetails = await _bookingService.GetTourDetailsAsync(_tourId);
            _tourName = tourDetails.TourName;
            _destination = tourDetails.Destination;
        }
        catch (Exception ex)
        {
            
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            });
        }
    }

    private async Task ConfirmBookingAsync()
    {
        // Валидация Email
        if (string.IsNullOrWhiteSpace(Email))
        {
            MessageBox.Show("Введите Email!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        
        if (NumberOfSeats <= 0)
        {
            MessageBox.Show("Количество мест должно быть больше нуля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            // Обновление Email пользователя
            await _emailService.UpdateUserEmailAsync(_userId, Email);

            // Добавление бронирования
            await _bookingService.AddBookingAsync(_userId, _tourId, NumberOfSeats, TotalPrice);

            // Создание PDF билета
            var ticketInfo = new TicketInfo
            {
                UserName = _userName,
                UserEmail = Email,
                TourName = _tourName,
                Destination = _destination,
                BookingDate = DateTime.Now,
                TourStartDate = DateTime.Now.AddDays(7), // Примерная дата начала тура
                NumberOfSeats = NumberOfSeats,
                TotalPrice = TotalPrice
            };

            // Генерация билета с использованием сервиса
            string ticketPath = _pdfTicketService.GenerateTicket(ticketInfo);

            MessageBox.Show(
                $"Бронирование успешно!\nВаш билет создан: {ticketPath}",
                "Успех",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
            RequestClose?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}


public interface IPdfTicketService
{
    string GenerateTicket(TicketInfo ticketInfo);
}

// DTO для передачи информации о билете
public class TicketInfo
{
    public string UserName { get; set; }
    public string UserEmail { get; set; }
    public string TourName { get; set; }
    public string Destination { get; set; }
    public DateTime BookingDate { get; set; }
    public DateTime TourStartDate { get; set; }
    public int NumberOfSeats { get; set; }
    public decimal TotalPrice { get; set; }
}