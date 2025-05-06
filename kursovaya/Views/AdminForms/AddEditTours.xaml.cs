using kursovaya.ViewModels;
using Interfaces.Services;
using DTO;
using System.Windows;
using BLL.Services;

namespace kursovaya.Views
{
    public partial class AddEditTours : Window
    {
        public AddEditTours(TourService tourService, TourDTO tourToEdit = null)
        {
            InitializeComponent();
            DataContext = new AddTourViewModel(tourService, tourToEdit);

            // Устанавливаем действие закрытия окна
            if (DataContext is AddTourViewModel viewModel)
            {
                viewModel.CloseAction = () => DialogResult = true;
            }
        }
    }
}