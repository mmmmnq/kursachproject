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
    }
}
