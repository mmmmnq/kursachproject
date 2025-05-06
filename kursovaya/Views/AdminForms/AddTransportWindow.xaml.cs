using BLL.Services;
using DTO;
using kursovaya.ViewModels;
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
    public partial class AddTransportWindow : Window
    {
        public AddTransportWindow(TransportService transportService, TransportDTO transportToEdit = null)
        {
            InitializeComponent();
            var viewModel = new AddTransportViewModel(transportService, transportToEdit);
            viewModel.CloseAction = Close;
            DataContext = viewModel;
        }
    }
}
