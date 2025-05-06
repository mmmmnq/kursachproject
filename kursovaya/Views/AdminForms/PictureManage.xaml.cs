using System.Windows;
using kursovaya.ViewModel; // Пространство имен для ViewModel

namespace kursovaya.Views
{
    public partial class ManageImagesWindow : Window
    {
        public ManageImagesWindow()
        {
            InitializeComponent();

            // Установка DataContext
            this.DataContext = new ManageImagesViewModel();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
