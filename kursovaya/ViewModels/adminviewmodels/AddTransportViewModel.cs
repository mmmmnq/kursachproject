using BLL.Services;
using DTO;
using kursovaya.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace kursovaya.ViewModels
{
    public class AddTransportViewModel : BaseViewModel
    {
        private readonly TransportDTO _transportToEdit;
        private readonly TransportService _transportService;
        private bool _isEditMode;


        private int _transportId;
        private string _type;
        private int _capacity;
        private string _company;
        private decimal _price;


        public int TransportId
        {
            get => _transportId;
            set
            {
                _transportId = value;
                OnPropertyChanged(nameof(TransportId));
            }
        }
        public string Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        public int Capacity
        {
            get => _capacity;
            set
            {
                _capacity = value;
                OnPropertyChanged(nameof(Capacity));
            }
        }

        public string Company
        {
            get => _company;
            set
            {
                _company = value;
                OnPropertyChanged(nameof(Company));
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

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public Action CloseAction { get; set; }

        public AddTransportViewModel(TransportService transportService, TransportDTO transportToEdit = null)
        {
            _transportService = transportService;
            _transportToEdit = transportToEdit;
            _isEditMode = transportToEdit != null;

            if (_isEditMode)
            {
                // Заполняем поля данными редактируемого транспорта
                TransportId = transportToEdit.TransportId;
                Type = transportToEdit.Type;
                Capacity = transportToEdit.Capacity;
                Company = transportToEdit.Company;
                Price = transportToEdit.Price;
            }

            SaveCommand = new RelayCommand(async _ => await ExecuteSaveAsync());
            CancelCommand = new RelayCommand(_ => ExecuteCancel());
        }


        private async Task ExecuteSaveAsync()
        {
            try
            {
                var transport = new TransportDTO
                {
                    TransportId = TransportId,
                    Type = Type,
                    Capacity = Capacity,
                    Company = Company,
                    Price = Price
                };

                if (_isEditMode)
                {
                    await _transportService.UpdateTransportAsync(transport);
                    MessageBox.Show("Транспорт успешно обновлен!");
                }
                else
                {
                    await _transportService.AddTransportAsync(transport);
                    MessageBox.Show("Транспорт успешно добавлен!");
                }

                CloseAction?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
        }

        private void ExecuteCancel()
        {
            CloseAction?.Invoke();
        }
    }
}