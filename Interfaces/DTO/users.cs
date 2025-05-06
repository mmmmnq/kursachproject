using System;
using System.ComponentModel;
using DomainModel;

namespace DTO
{
    public class UserDTO : INotifyPropertyChanged
    {
        private int userId;
        private string firstName;
        private string lastName;
        private string phoneNumber;
        private string email;
        private string role;
        private string username;
        private string password;

        public event PropertyChangedEventHandler PropertyChanged;

        public int UserId
        {
            get => userId;
            set
            {
                if (userId != value)
                {
                    userId = value;
                    OnPropertyChanged(nameof(UserId));
                }
            }
        }

        public string FirstName
        {
            get => firstName;
            set
            {
                if (firstName != value)
                {
                    firstName = value;
                    OnPropertyChanged(nameof(FirstName));
                }
            }
        }

        public string LastName
        {
            get => lastName;
            set
            {
                if (lastName != value)
                {
                    lastName = value;
                    OnPropertyChanged(nameof(LastName));
                }
            }
        }

        public string PhoneNumber
        {
            get => phoneNumber;
            set
            {
                if (phoneNumber != value)
                {
                    phoneNumber = value;
                    OnPropertyChanged(nameof(PhoneNumber));
                }
            }
        }

        public string Email
        {
            get => email;
            set
            {
                if (email != value)
                {
                    email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        public string Role
        {
            get => role;
            set
            {
                if (role != value)
                {
                    role = value;
                    OnPropertyChanged(nameof(Role));
                }
            }
        }

        public string Username
        {
            get => username;
            set
            {
                if (username != value)
                {
                    username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        public string Password  // Добавляем свойство для пароля
        {
            get => password;
            set
            {
                if (password != value)
                {
                    password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        // Метод для уведомления об изменениях
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public UserDTO() { }

        public UserDTO(users user)
        {
            UserId = user.userid;
            FirstName = user.firstname;
            LastName = user.lastname;
            PhoneNumber = user.phonenumber;
            Email = user.email;
            Role = user.role;
            Username = user.username;
            Password = user.password;
        }
    }
}
