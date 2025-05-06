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
    /// Логика взаимодействия для AdminForm.xaml
    /// </summary>
    public partial class AdminForm : Window
    {
        public AdminForm()
        {
            InitializeComponent();
            this.DataContext = new ViewModels.AdminViewModel();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            var loginWindow = new MainWindow();
           
            loginWindow.Show();
            
            this.Close();
        }
    }
}
