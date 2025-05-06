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
    public class ManageUsersViewModel : BaseViewModel
    {
        private readonly IUserService _userService;
        private ObservableCollection<UserDTO> _users;
        private UserDTO _selectedUser;
        private bool _isEditing;
        private UserDTO _originalUser;
        private readonly UserService _concreteService;
        public ObservableCollection<UserDTO> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
            }
        }

        public UserDTO SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
                // Update commands that depend on SelectedUser
                (EditUserCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (DeleteUserCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (SaveUserCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (CancelCommand as RelayCommand)?.RaiseCanExecuteChanged();
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

        public ICommand AddUserCommand { get; }
        public ICommand EditUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand SaveUserCommand { get; }
        public ICommand CancelCommand { get; }

        public ManageUsersViewModel(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            Users = new ObservableCollection<UserDTO>();

            // Initialize commands
            AddUserCommand = new RelayCommand(_ => ExecuteAddUser());
            EditUserCommand = new RelayCommand(_ => ExecuteEditUser(), _ => SelectedUser != null);
            DeleteUserCommand = new RelayCommand(async _ => await ExecuteDeleteUserAsync(), _ => SelectedUser != null);
            SaveUserCommand = new RelayCommand(async _ => await ExecuteSaveUserAsync(), _ => SelectedUser != null);
            CancelCommand = new RelayCommand(_ => ExecuteCancel(), _ => IsEditing);

            // Load users
            _ = LoadUsersAsync();
        }

        private async Task LoadUsersAsync()
        {
            try
            {
                var users = await _userService.GetUsersAsync();
                Users = new ObservableCollection<UserDTO>(users);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке пользователей: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteAddUser()
        {
            try
            {
                var addEditWindow = new AddEditUserWindow(_userService)
                {
                    Owner = Application.Current.MainWindow
                };

                if (addEditWindow.ShowDialog() == true)
                {
                    _ = LoadUsersAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии формы: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteEditUser()
        {
            if (SelectedUser != null)
            {
                try
                {
                    // Создаем копию выбранного пользователя для редактирования
                    var userToEdit = new UserDTO
                    {
                        UserId = SelectedUser.UserId,
                        Username = SelectedUser.Username,
                        FirstName = SelectedUser.FirstName,
                        LastName = SelectedUser.LastName,
                        Email = SelectedUser.Email,
                        PhoneNumber = SelectedUser.PhoneNumber,
                        Role = SelectedUser.Role,
                        Password = SelectedUser.Password
                    };

                    // Создаем окно редактирования, передавая сервис и пользователя
                    var editWindow = new AddEditUserWindow(_userService, userToEdit)
                    {
                        Owner = Application.Current.MainWindow,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    };
                    
                    // После закрытия окна обновляем список пользователей


                    if (editWindow.ShowDialog() == true)
                    {
                        _ = LoadUsersAsync();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при открытии формы редактирования: {ex.Message}",
                                  "Ошибка",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
                }
            }
        }

        private async Task ExecuteDeleteUserAsync()
        {
            if (SelectedUser == null) return;

            var result = MessageBox.Show("Вы уверены, что хотите удалить этого пользователя?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    bool success = await _userService.DeleteUserAsync(SelectedUser.UserId);
                    if (success)
                    {
                        Users.Remove(SelectedUser);
                        SelectedUser = null;
                        MessageBox.Show("Пользователь успешно удален", "Успех",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении пользователя: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task ExecuteSaveUserAsync()
        {
            if (!ValidateUser(SelectedUser)) return;

            try
            {
                bool success;
                if (SelectedUser.UserId == 0)
                {
                    success = await _userService.AddUserAsync(SelectedUser);
                    if (success) Users.Add(SelectedUser);
                }
                else
                {
                    success = await _userService.UpdateUserAsync(SelectedUser);
                    if (success)
                    {
                        var index = Users.IndexOf(Users.First(u => u.UserId == SelectedUser.UserId));
                        Users[index] = SelectedUser;
                    }
                }

                if (success)
                {
                    IsEditing = false;
                    _originalUser = null;
                    MessageBox.Show("Пользователь успешно сохранен", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении пользователя: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteCancel()
        {
            if (_originalUser != null && SelectedUser != null)
            {
                // Restore original values
                var properties = typeof(UserDTO).GetProperties();
                foreach (var prop in properties)
                {
                    prop.SetValue(SelectedUser, prop.GetValue(_originalUser));
                }
            }

            IsEditing = false;
            _originalUser = null;
        }

        private bool ValidateUser(UserDTO user)
        {
            if (user == null) return false;

            if (string.IsNullOrWhiteSpace(user.Username))
            {
                MessageBox.Show("Введите логин пользователя", "Ошибка валидации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(user.FirstName))
            {
                MessageBox.Show("Введите имя пользователя", "Ошибка валидации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(user.LastName))
            {
                MessageBox.Show("Введите фамилию пользователя", "Ошибка валидации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(user.Email) || !user.Email.Contains("@"))
            {
                MessageBox.Show("Введите корректный email", "Ошибка валидации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(user.PhoneNumber))
            {
                MessageBox.Show("Введите номер телефона", "Ошибка валидации",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
    }
}