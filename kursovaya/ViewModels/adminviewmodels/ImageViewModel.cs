using BLL.Services;
using DTO;
using kursovaya.ViewModels;
using kursovaya.Views;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using kursovaya.Helpers;

namespace kursovaya.ViewModel
{
    public class ManageImagesViewModel : BaseViewModel
    {
        private readonly ImageService _imageService;
        private ObservableCollection<ImageDTO> _images;
        private ImageDTO _selectedImage;


        public ObservableCollection<ImageDTO> Images
        {
            get => _images;
            set
            {
                _images = value;
                OnPropertyChanged(nameof(Images));
            }
        }

        public ImageDTO SelectedImage
        {
            get => _selectedImage;
            set
            {
                _selectedImage = value;
                OnPropertyChanged(nameof(SelectedImage));
                // Обновляем состояние команд при изменении выбранного изображения
                (EditImageCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (DeleteImageCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ICommand EditImageCommand { get; }
        public ICommand DeleteImageCommand { get; }
        public ICommand OpenAddImageWindowCommand { get; }

        public ManageImagesViewModel()
        {
            _imageService = new ImageService("Host=localhost;Port=5432;Username=postgres;Password=123;Database=TravelAgency");
            Images = new ObservableCollection<ImageDTO>();

            LoadImagesAsync();

            EditImageCommand = new RelayCommand(ExecuteEditImage, CanExecuteEditImage);
            DeleteImageCommand = new RelayCommand(async _ => await ExecuteDeleteImageAsync(), CanExecuteDeleteImage);
            OpenAddImageWindowCommand = new RelayCommand(ExecuteOpenAddImageWindow);
        }

        private async Task LoadImagesAsync()
        {
            try
            {
                var images = await _imageService.GetAllImagesAsync();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Images.Clear();
                    foreach (var image in images)
                    {
                        Images.Add(image);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
            }
        }

        private void ExecuteEditImage(object parameter)
        {
            if (SelectedImage != null)
            {
                var editWindow = new AddImageWindow(_imageService);
                var viewModel = new AddImageViewModel(_imageService,
                    async () =>
                    {
                        await LoadImagesAsync();
                        editWindow.Close();
                    },
                    SelectedImage);
                editWindow.DataContext = viewModel;
                editWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите изображение для редактирования.");
            }
        }


        private bool CanExecuteEditImage(object parameter)
        {
            return SelectedImage != null;
        }

        private async Task ExecuteDeleteImageAsync()
        {
            if (SelectedImage != null)
            {
                try
                {
                    await _imageService.DeleteImageAsync(SelectedImage.ImageId);

                    // Удаляем файл изображения
                    if (System.IO.File.Exists(SelectedImage.ImagePath))
                    {
                        System.IO.File.Delete(SelectedImage.ImagePath);
                    }

                    Images.Remove(SelectedImage);
                    MessageBox.Show("Изображение успешно удалено!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении изображения: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Выберите изображение для удаления.");
            }
        }

        private bool CanExecuteDeleteImage(object parameter)
        {
            return SelectedImage != null;
        }

        private void ExecuteOpenAddImageWindow(object parameter)
        {
            var addImageWindow = new AddImageWindow(_imageService);
            addImageWindow.Closed += async (s, e) => await LoadImagesAsync();
            addImageWindow.ShowDialog();
        }


    }
}
