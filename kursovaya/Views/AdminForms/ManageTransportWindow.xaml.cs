﻿using kursovaya.ViewModel;
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
    /// Логика взаимодействия для ManageTransportWindow.xaml
    /// </summary>
    public partial class ManageTransportWindow : Window
    {
        public ManageTransportWindow()
        {
            InitializeComponent();
            DataContext = new ManageTransportViewModel();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

}
