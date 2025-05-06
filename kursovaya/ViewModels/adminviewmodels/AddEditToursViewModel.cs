using BLL.Managers;
using BLL.Services;
using DTO;
using kursovaya.Helpers;
using System;
using System.Collections.ObjectModel; // Для ObservableCollection
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace kursovaya.ViewModels
{
    public class AddTourViewModel : BaseViewModel
    {
        private readonly TourManager _tourManager;
        private readonly TourDTO _tourToEdit;
        private bool _isEditMode;

        private int _tourId;
        private string _name = string.Empty;
        private string _description;
        private decimal _price;
        private DateTime _startDate;
        private DateTime _endDate;
        private string _destination;
        private int _availableSeats;
        private string _status;
        private int? _transportId;

        // Новое свойство для хранения допустимых значений статусов
        public ObservableCollection<string> StatusOptions { get; } = new ObservableCollection<string>();

        public int TourId
        {
            get => _tourId;
            set
            {
                _tourId = value;
                OnPropertyChanged(nameof(TourId));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value ?? string.Empty;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }

        public string Destination
        {
            get => _destination;
            set
            {
                _destination = value;
                OnPropertyChanged(nameof(Destination));
            }
        }

        public int? TransportId
        {
            get => _transportId;
            set
            {
                _transportId = value;
                OnPropertyChanged(nameof(TransportId));
            }
        }

        public int AvailableSeats
        {
            get => _availableSeats;
            set
            {
                _availableSeats = value;
                OnPropertyChanged(nameof(AvailableSeats));
            }
        }

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action CloseAction { get; set; }

        public AddTourViewModel(TourService tourService, TourDTO tourToEdit = null)
        {
            _tourManager = new TourManager(tourService);
            _tourToEdit = tourToEdit;
            _isEditMode = tourToEdit != null;

            // Заполняем коллекцию допустимых значений статусов
            LoadStatusOptions();

            if (_isEditMode)
            {
                TourId = tourToEdit.TourId;
                Name = tourToEdit.Name ?? string.Empty;
                Description = tourToEdit.Description;
                Price = tourToEdit.Price;
                StartDate = tourToEdit.StartDate;
                EndDate = tourToEdit.EndDate;
                Destination = tourToEdit.Destination;
                TransportId = tourToEdit.TransportId;
                AvailableSeats = tourToEdit.AvailableSeats;
                Status = tourToEdit.Status;
            }
            else
            {
                Name = string.Empty;
                StartDate = DateTime.Today;
                EndDate = DateTime.Today.AddDays(7);
                Status = "Активен";
            }

            SaveCommand = new RelayCommand(async _ => await ExecuteSaveAsync());
            CancelCommand = new RelayCommand(_ => ExecuteCancel());
        }

        // Метод для загрузки допустимых значений статусов
        private void LoadStatusOptions()
        {
            // Здесь вы можете динамически загружать значения, например, из базы данных или сервиса
            StatusOptions.Clear();
            StatusOptions.Add("Активен");
            StatusOptions.Add("Завершен");
            StatusOptions.Add("Отменен");
        }

        private async Task ExecuteSaveAsync()
        {
            try
            {
                var tourData = new TourDTO
                {
                    TourId = TourId,
                    Name = Name,
                    Description = Description,
                    Price = Price,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    Destination = Destination,
                    TransportId = TransportId,
                    AvailableSeats = AvailableSeats,
                    Status = Status
                };

                var (success, message) = _isEditMode
                    ? await _tourManager.UpdateTourAsync(tourData)
                    : await _tourManager.CreateTourAsync(tourData);

                MessageBox.Show(message);

                if (success)
                {
                    CloseAction?.Invoke();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void ExecuteCancel()
        {
            CloseAction?.Invoke();
        }
    }
}
