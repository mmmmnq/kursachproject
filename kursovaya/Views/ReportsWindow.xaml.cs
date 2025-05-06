using System.Collections.Generic;
using System.Windows;
using DTO;
using kursovaya.ViewModels;

namespace kursovaya.Views
{
    public partial class ReportsWindow : Window
    {
        public ReportsWindow(IEnumerable<BookingDTO> bookings, IEnumerable<TourDTO> tours)
        {
            InitializeComponent();
            if (bookings == null || tours == null)
            {
                MessageBox.Show("Не удалось загрузить данные");
                Close();
                return;
            }
            DataContext = new ReportViewModel(bookings, tours);
        }
    }
}