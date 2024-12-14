using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BLL;
using kursovaya.Helpers;

namespace kursovaya.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _username;
        private string _password;
        private readonly IUserService _userService;

        public LoginViewModel()
        {
            _userService = new UserService();
            LoginCommand = new RelayCommand(ExecuteLogin);
            RegisterCommand = new RelayCommand(ExecuteRegister);
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        private void ExecuteLogin(object parameter)
        {
            string role;
            var isAuthenticated = _userService.AuthenticateUser(Username, Password, out role);

            if (isAuthenticated)
            {
                // Установить текущего пользователя
                var userId = _userService.GetUserId(Username); // Получение ID пользователя
                CurrentUser.SetUser(userId, Username, role);

                // Открытие соответствующего окна на основе роли
                switch (role.ToLower())
                {
                    case "admin":
                        OpenAdminWindow();
                        break;
                    case "user":
                        OpenUserWindow();
                        break;
                    default:
                        MessageBox.Show("Неизвестная роль пользователя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
            }
            else
            {
                MessageBox.Show("Неверное имя пользователя или пароль.");
            }
        }


        private void ExecuteRegister(object parameter)
        {
            // Логика для открытия окна регистрации
            var registerWindow = new Views.RegisterForm();
            registerWindow.Show();
        }

        private void OpenAdminWindow()
        {
            var adminWindow = new Views.AdminForm();
            adminWindow.Show();
            CloseCurrentWindow();
        }

        private void OpenUserWindow()
        {
            var userWindow = new Views.UserForm();
            userWindow.Show();
            CloseCurrentWindow();
        }

        public void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            Password = passwordBox?.Password;
        }
        private void CloseCurrentWindow()
        {
            Application.Current.MainWindow?.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
