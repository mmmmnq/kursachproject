using System;
using System.Windows;
using kursovaya.ViewModel;
using BLL.Services;
using DTO;

namespace kursovaya.Views
{
    public partial class AddImageWindow : Window
    {
        public AddImageWindow(ImageService imageService, ImageDTO editingImage = null)
        {
            InitializeComponent();
            DataContext = new AddImageViewModel(imageService, Close, editingImage);
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

}
