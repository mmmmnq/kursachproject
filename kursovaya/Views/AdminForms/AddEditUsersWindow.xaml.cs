using kursovaya.ViewModels;
using Interfaces.Services;
using DTO;
using System.Windows;

namespace kursovaya.Views
{
    public partial class AddEditUserWindow : Window
    {
        public AddEditUserWindow(IUserService userService, UserDTO user = null)
        {
            InitializeComponent();
            PasswordBox.PasswordChanged += PasswordBox_PasswordChanged;
            DataContext = new AddEditUserViewModel(userService, user);
            if (DataContext is AddEditUserViewModel viewModel)
            {
                viewModel.CloseAction = () => DialogResult = true;
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is AddEditUserViewModel viewModel)
            {
                viewModel.Password = PasswordBox.Password;
            }
        }

    }

}