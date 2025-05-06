using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DTO;
using Interfaces.Services;
using BLL.Services;

namespace kursovaya.ViewModels
{
    public class ReviewsViewModel : INotifyPropertyChanged
    {
        private readonly ITourService _tourService;

        private ObservableCollection<TourWithImageDTO> _tours;
        public ObservableCollection<TourWithImageDTO> Tours
        {
            get => _tours;
            set
            {
                _tours = value;
                OnPropertyChanged();
            }
        }

        private int _tourId;
        public int TourId
        {
            get => _tourId;
            set
            {
                _tourId = value;
                OnPropertyChanged();
            }
        }

        public ReviewsViewModel(int tourId)
        {
            TourId = tourId;
            _tourService = new TourService("Host=localhost;Port=5432;Username=postgres;Password=123;Database=TravelAgency");
            Tours = new ObservableCollection<TourWithImageDTO>();
            LoadTours();
        }

        private async void LoadTours()
        {
            // Загружаем данные из сервиса
            var tourDtos = await _tourService.GetToursAsync();
            Tours.Clear(); // Очищаем коллекцию перед добавлением

            foreach (var tour in tourDtos)
            {
                Tours.Add(tour);
            }
        }

        // Реализация INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
