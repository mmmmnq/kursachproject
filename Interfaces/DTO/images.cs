using System;
using System.ComponentModel;
using DomainModel;

namespace DTO
{
    public class ImageDTO : INotifyPropertyChanged
    {
        private int imageId;
        private int? tourId;
        private string imagePath;

        public event PropertyChangedEventHandler PropertyChanged;

        public int ImageId
        {
            get => imageId;
            set
            {
                imageId = value;
                OnPropertyChanged(nameof(ImageId));
            }
        }

        public int? TourId
        {
            get => tourId;
            set
            {
                tourId = value;
                OnPropertyChanged(nameof(TourId));
            }
        }

        public string ImagePath
        {
            get => imagePath;
            set
            {
                imagePath = value;
                OnPropertyChanged(nameof(ImagePath));
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ImageDTO() { }

        public ImageDTO(images image)
        {
            ImageId = image.imageid;
            TourId = image.tourid;
            ImagePath = image.imagepath;
        }
    }
}
