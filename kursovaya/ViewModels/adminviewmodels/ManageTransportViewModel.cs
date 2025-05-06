using BLL.Services;
using DTO;
using kursovaya.Helpers;
using kursovaya.ViewModels;
using kursovaya.Views;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System;

public class ManageTransportViewModel : BaseViewModel
{
    private readonly TransportService _transportService;
    private ObservableCollection<TransportDTO> _transports;
    private TransportDTO _selectedTransport;

    public ObservableCollection<TransportDTO> Transports
    {
        get => _transports;
        set
        {
            _transports = value;
            OnPropertyChanged(nameof(Transports));
        }
    }

    public TransportDTO SelectedTransport
    {
        get => _selectedTransport;
        set
        {
            _selectedTransport = value;
            OnPropertyChanged(nameof(SelectedTransport));
            // Обновляем состояние команд при изменении выбранного транспорта
            (EditTransportCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (DeleteTransportCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }
    }

    public ICommand EditTransportCommand { get; }
    public ICommand DeleteTransportCommand { get; }
    public ICommand OpenAddTransportWindowCommand { get; }

    public ManageTransportViewModel()
    {
        _transportService = new TransportService("Host=localhost;Port=5432;Username=postgres;Password=123;Database=TravelAgency");
        Transports = new ObservableCollection<TransportDTO>();

        LoadTransportsAsync();

        EditTransportCommand = new RelayCommand(ExecuteEditTransport, CanExecuteEditTransport);
        DeleteTransportCommand = new RelayCommand(async _ => await ExecuteDeleteTransportAsync(), CanExecuteDeleteTransport);
        OpenAddTransportWindowCommand = new RelayCommand(ExecuteOpenAddTransportWindow);
    }

    private async Task LoadTransportsAsync()
    {
        try
        {
            var transports = await _transportService.GetAllTransportsAsync();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Transports.Clear();
                foreach (var transport in transports)
                {
                    Transports.Add(transport);
                }
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ExecuteEditTransport(object parameter)
    {
        if (SelectedTransport != null)
        {
            var editWindow = new AddTransportWindow(_transportService, SelectedTransport)
            {
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            if (editWindow.ShowDialog() == true)
            {
                _ = LoadTransportsAsync();
            }
        }
        else
        {
            MessageBox.Show("Выберите транспорт для редактирования.", "Предупреждение",
                MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private bool CanExecuteEditTransport(object parameter)
    {
        return SelectedTransport != null;
    }

    private async Task ExecuteDeleteTransportAsync()
    {
        if (SelectedTransport == null) return;

        var result = MessageBox.Show("Вы уверены, что хотите удалить этот транспорт?", "Подтверждение",
            MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            try
            {
                await _transportService.DeleteTransportAsync(SelectedTransport.TransportId);
                Transports.Remove(SelectedTransport);
                SelectedTransport = null;
                MessageBox.Show("Транспорт успешно удален!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении транспорта: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private bool CanExecuteDeleteTransport(object parameter)
    {
        return SelectedTransport != null;
    }

    private void ExecuteOpenAddTransportWindow(object parameter)
    {
        var addTransportWindow = new AddTransportWindow(_transportService)
        {
            Owner = Application.Current.MainWindow,
            WindowStartupLocation = WindowStartupLocation.CenterScreen
        };

        if (addTransportWindow.ShowDialog() == true)
        {
            _ = LoadTransportsAsync();
        }
    }
}