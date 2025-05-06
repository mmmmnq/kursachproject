using BLL.Services;
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
using BLL.Repositories;
using Interfaces.Services;
using kursovaya.ViewModels;
namespace kursovaya.Views
{
    /// <summary>
    /// Логика взаимодействия для ManageTourWindow.xaml
    /// </summary>
    public partial class ManageTourWindow : Window
    {
        public ManageTourWindow(ITourService tourService)
        {
            InitializeComponent();
            DataContext = new ManageTourViewModel(tourService);

        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
