using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using kursovaya.Helpers;
using DTO;
using BLL.Services;
using kursovaya.ViewModels;
using System.Threading.Tasks;

namespace kursovaya.ViewModel
{
    public class AddImageViewModel : BaseViewModel
    {
        private readonly ImageService _imageService;
        private readonly Action _onSave;
        private ImageDTO _editingImage;
        public ICommand BrowseCommand { get; }


        public string WindowTitle => _editingImage == null ? "Добавление изображения" : "Редактирование изображения";
        public int? TourId { get; set; }
        public string ImagePath { get; set; }

        public ICommand SaveCommand { get; }
        

        public AddImageViewModel(ImageService imageService, Action onSave, ImageDTO editingImage = null)
        {
            _imageService = imageService;
            _onSave = onSave;
            _editingImage = editingImage;
            BrowseCommand = new RelayCommand(_ => ExecuteBrowse());
            if (_editingImage != null)
            {
                TourId = _editingImage.TourId;
                ImagePath = _editingImage.ImagePath;
            }

            SaveCommand = new RelayCommand(async _ => await ExecuteSaveAsync());
            
        }
        private void ExecuteBrowse()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Изображения|*.jpg;*.jpeg;*.png;*.gif;*.bmp|Все файлы|*.*",
                Title = "Выберите изображение"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ImagePath = openFileDialog.FileName;
                OnPropertyChanged(nameof(ImagePath));
            }
        }
        private async Task ExecuteSaveAsync()
        {
            if (string.IsNullOrEmpty(ImagePath) || !File.Exists(ImagePath))
            {
                MessageBox.Show("Выберите корректный путь к изображению.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var imageDto = new ImageDTO
                {
                    ImageId = _editingImage?.ImageId ?? 0,
                    ImagePath = ImagePath,
                    TourId = TourId
                };

                if (_editingImage == null)
                {
                    await _imageService.AddImageAsync(imageDto);
                    MessageBox.Show("Изображение успешно добавлено!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    await _imageService.UpdateImageAsync(imageDto);
                    MessageBox.Show("Изображение успешно обновлено!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }

                _onSave?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении изображения: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        
    }
}
