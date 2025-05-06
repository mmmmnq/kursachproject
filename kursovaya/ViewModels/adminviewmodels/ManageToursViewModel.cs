// ManageTourViewModel.cs
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using DTO;
using Interfaces.Services;
using kursovaya.Helpers;
using System.Linq;
using System.Windows;
using System;
using kursovaya.Views;
using BLL.Services;

namespace kursovaya.ViewModels
{
    public class ManageTourViewModel : BaseViewModel
    {
        private readonly ITourService _tourService;
        private ObservableCollection<TourDTO> _tours;
        private TourDTO _selectedTour;
        private bool _isEditing;
        private TourDTO _originalTour;
        private readonly TourService _concreteService;
        public ObservableCollection<TourDTO> Tours
        {
            get => _tours;
            set
            {
                _tours = value;
                OnPropertyChanged(nameof(Tours));
            }
        }

        public TourDTO SelectedTour
        {
            get => _selectedTour;
            set
            {
                _selectedTour = value;
                OnPropertyChanged(nameof(SelectedTour));
                // Update commands that depend on SelectedTour
                (EditTourCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (DeleteTourCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = value;
                OnPropertyChanged(nameof(IsEditing));
            }
        }

        public ICommand AddTourCommand { get; }
        public ICommand EditTourCommand { get; }
        public ICommand DeleteTourCommand { get; }
        public ICommand SaveTourCommand { get; }
        public ICommand CancelCommand { get; }

        public ManageTourViewModel(ITourService tourService)
        {
            _tourService = tourService ?? throw new ArgumentNullException(nameof(tourService));
            _concreteService = tourService as TourService ?? throw new ArgumentException("Expected TourService implementation", nameof(tourService));
            Tours = new ObservableCollection<TourDTO>();

            // Initialize commands
            AddTourCommand = new RelayCommand(_ => ExecuteAddTour());
            EditTourCommand = new RelayCommand(_ => ExecuteEditTour(), _ => SelectedTour != null);
            DeleteTourCommand = new RelayCommand(async _ => await ExecuteDeleteTourAsync(), _ => SelectedTour != null);
            SaveTourCommand = new RelayCommand(async _ => await ExecuteSaveTourAsync());
            CancelCommand = new RelayCommand(_ => ExecuteCancel());

            // Load tours
            _ = LoadToursAsync();
        }

        private async Task LoadToursAsync()
        {
            try
            {
                var tours = await _tourService.GetToursAsync();
                // Преобразование TourWithImageDTO в TourDTO
                var tourDtos = tours.Select(t => new TourDTO
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
                    TransportId = t.TransportID
                }).ToList();

                Tours = new ObservableCollection<TourDTO>(tourDtos);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке туров: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteAddTour()
        {
            try
            {
                var addEditWindow = new AddEditTours(_concreteService) // Используем конкретную реализацию
                {
                    Owner = Application.Current.MainWindow
                };

                if (addEditWindow.ShowDialog() == true)
                {
                    _ = LoadToursAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии формы: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteEditTour()
        {
            if (SelectedTour != null)
            {
                try
                {
                    var addEditWindow = new AddEditTours(_concreteService, SelectedTour) // Передаем выбранный тур
                    {
                        Owner = Application.Current.MainWindow
                    };

                    if (addEditWindow.ShowDialog() == true)
                    {
                        _ = LoadToursAsync(); // Перезагружаем список туров после редактирования
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при открытии формы редактирования: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task ExecuteDeleteTourAsync()
        {
            if (SelectedTour == null) return;

            var result = MessageBox.Show("Вы уверены, что хотите удалить этот тур?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    if (await _tourService.DeleteTourAsync(SelectedTour.TourId))
                    {
                        Tours.Remove(SelectedTour);
                        SelectedTour = null;
                        MessageBox.Show("Тур успешно удален", "Успех",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении тура: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task ExecuteSaveTourAsync()
        {
            if (!ValidateTour(SelectedTour)) return;

            try
            {
                bool success;
                if (SelectedTour.TourId == 0)
                {
                    success = await _tourService.AddTourAsync(SelectedTour);
                    if (success) Tours.Add(SelectedTour);
                }
                else
                {
                    success = await _tourService.UpdateTourAsync(SelectedTour);
                    if (success)
                    {
                        var index = Tours.IndexOf(Tours.First(t => t.TourId == SelectedTour.TourId));
                        Tours[index] = SelectedTour;
                    }
                }

                if (success)
                {
                    IsEditing = false;
                    _originalTour = null;
                    MessageBox.Show("Тур успешно сохранен", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении тура: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteCancel()
        {
            if (_originalTour != null && SelectedTour != null)
            {
                // Restore original values
                var properties = typeof(TourDTO).GetProperties();
                foreach (var prop in properties)
                {
                    prop.SetValue(SelectedTour, prop.GetValue(_originalTour));
                }
            }

            IsEditing = false;
            _originalTour = null;
        }

        private bool ValidateTour(TourDTO tour)
        {
            if (tour == null) return false;

            if (string.IsNullOrWhiteSpace(tour.Name))
            {
                MessageBox.Show("Введите название тура", "Ошибка валидации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (tour.Price <= 0)
            {
                MessageBox.Show("Цена должна быть больше нуля", "Ошибка валидации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (tour.EndDate <= tour.StartDate)
            {
                MessageBox.Show("Дата окончания должна быть позже даты начала", "Ошибка валидации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (tour.AvailableSeats <= 0)
            {
                MessageBox.Show("Количество мест должно быть больше нуля", "Ошибка валидации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
    }
}