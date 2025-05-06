using System.Windows;
using kursovaya.ViewModels;

namespace kursovaya.Views
{
    public partial class UserForm : Window
    {
        public UserForm()
        {
            InitializeComponent();

            // Создание экземпляра TourViewModel
            var viewModel = new TourViewModel();

            // Установка контекста данных
            this.DataContext = viewModel;
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // Создаем и показываем окно авторизации
            var loginWindow = new MainWindow();
            loginWindow.Show();

            // Закрываем текущее окно
            this.Close();
        }
    
    }
}
