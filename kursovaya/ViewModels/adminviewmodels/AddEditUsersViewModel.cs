using DTO;
using Interfaces.Services;
using kursovaya.Helpers;
using kursovaya.ViewModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System;

public class AddEditUserViewModel : BaseViewModel
{
    private readonly IUserService _userService;
    private readonly UserDTO _userToEdit;
    private bool _isEditMode;

    private int _userId;
    private string _username = string.Empty;
    private string _password;
    private string _email;
    private string _firstName;
    private string _lastName;
    private string _role;

    // Коллекция для хранения допустимых ролей
    public ObservableCollection<string> RoleOptions { get; } = new ObservableCollection<string>();

    public int UserId
    {
        get => _userId;
        set
        {
            _userId = value;
            OnPropertyChanged(nameof(UserId));
        }
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

    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged(nameof(Email));
        }
    }

    public string FirstName
    {
        get => _firstName;
        set
        {
            _firstName = value;
            OnPropertyChanged(nameof(FirstName));
        }
    }
    private string _phoneNumber;

    public string PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            _phoneNumber = value;
            OnPropertyChanged(nameof(PhoneNumber));
        }
    }
    public string LastName
    {
        get => _lastName;
        set
        {
            _lastName = value;
            OnPropertyChanged(nameof(LastName));
        }
    }

    public string Role
    {
        get => _role;
        set
        {
            _role = value;
            OnPropertyChanged(nameof(Role));
        }
    }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }
    public Action CloseAction { get; set; }

    public AddEditUserViewModel(IUserService userService, UserDTO userToEdit = null)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _userToEdit = userToEdit;
        _isEditMode = userToEdit != null;

        LoadRoleOptions();

        if (_isEditMode)
        {
            UserId = userToEdit.UserId;
            Username = userToEdit.Username ?? string.Empty;
            Email = userToEdit.Email ?? string.Empty;
            FirstName = userToEdit.FirstName ?? string.Empty;
            LastName = userToEdit.LastName ?? string.Empty;
            Role = userToEdit.Role ?? "User";
            Password = userToEdit.Password ?? string.Empty;
            PhoneNumber = userToEdit.PhoneNumber ?? string.Empty;
        }
        else
        {
            Username = string.Empty;
            Password = string.Empty;
            Email = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            PhoneNumber = string.Empty;
            Role = "User"; // Роль по умолчанию
        }

        SaveCommand = new RelayCommand(async _ => await ExecuteSaveAsync());
        CancelCommand = new RelayCommand(_ => ExecuteCancel());
    }

    private void LoadRoleOptions()
    {
        RoleOptions.Clear();
        RoleOptions.Add("Admin");
        RoleOptions.Add("User");
    }

    private void ExecuteCancel()
    {
        CloseAction?.Invoke();
    }

    private async Task ExecuteSaveAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                MessageBox.Show("Имя пользователя обязательно для заполнения");
                return;
            }

            if (!_isEditMode && string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Пароль обязателен для нового пользователя");
                return;
            }

            var userData = new UserDTO
            {
                UserId = UserId,
                Username = Username,
                Password = Password,
                Email = Email,
                FirstName = FirstName,
                LastName = LastName,
                PhoneNumber = PhoneNumber,
                Role = Role
            };

            bool success;
            string message;

            if (_isEditMode)
            {
                success = await _userService.UpdateUserAsync(userData);
                message = success ? "Пользователь успешно обновлен" : "Ошибка обновления пользователя";
            }
            else
            {
                success = await _userService.CreateUserAsync(userData);
                message = success ? "Пользователь успешно добавлен" : "Ошибка добавления пользователя";
            }

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
}
