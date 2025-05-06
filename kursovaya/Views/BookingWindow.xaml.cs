using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace kursovaya.Views
{
    /// <summary>
    /// Логика взаимодействия для BookingWindow.xaml
    /// </summary>
    public partial class BookingWindow : Window
    {
        public BookingWindow(int userId, int tourId, decimal tourPrice, decimal transportPrice, string connectionString)
        {
            InitializeComponent();

            var viewModel = new BookingViewModel(userId, tourId, tourPrice, transportPrice, connectionString);
            DataContext = viewModel;

            // Подписываемся на событие закрытия окна
            viewModel.RequestClose += (s, e) =>
            {
                // Закрываем окно в UI-потоке
                Dispatcher.Invoke(() => Close());
            };

        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

}
