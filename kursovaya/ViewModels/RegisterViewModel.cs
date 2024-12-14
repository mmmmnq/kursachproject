using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using kursovaya.Models;
using kursovaya.Helpers;
using kursovaya.Services;
using System.Configuration;

namespace kursovaya.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
       

        public ICommand RegisterCommand { get; }

        private readonly RegisterService _registerService;

        public RegisterViewModel()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["TravelAgencyDb"]?.ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
            {
                MessageBox.Show("Строка подключения не найдена.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _registerService = new RegisterService(connectionString);
            RegisterCommand = new RelayCommand(Register, CanRegister);
        }

        private void Register(object parameter)
        {
            var user = new User
            {
                Username = Username,
                Password = Password,
                FirstName = FirstName,
                LastName = LastName,
                Phone = Phone,
                
            };

            if (_registerService.RegisterUser(user, out string errorMessage))
            {
                MessageBox.Show("Регистрация успешна!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Ошибка: " + errorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private bool CanRegister(object parameter)
        {
            return !string.IsNullOrEmpty(Username) &&
                   !string.IsNullOrEmpty(Password) &&
                   !string.IsNullOrEmpty(FirstName) &&
                   !string.IsNullOrEmpty(LastName) &&
                   !string.IsNullOrEmpty(Phone);
                   
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
