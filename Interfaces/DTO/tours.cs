using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DomainModel;

namespace DTO
{
    public class TourDTO : INotifyPropertyChanged, ICloneable
    {
        private int _tourId;
        private string _name;
        private string _description;
        private decimal _price;
        private DateTime _startDate;
        private DateTime _endDate;
        private string _destination;
        private int _availableSeats;
        private string _status;
        private int? _transportId;

        public int TourId
        {
            get => _tourId;
            set
            {
                _tourId = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged();
            }
        }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged();
            }
        }

        public string Destination
        {
            get => _destination;
            set
            {
                _destination = value;
                OnPropertyChanged();
            }
        }

        public int AvailableSeats
        {
            get => _availableSeats;
            set
            {
                _availableSeats = value;
                OnPropertyChanged();
            }
        }

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        public int? TransportId
        {
            get => _transportId;
            set
            {
                _transportId = value;
                OnPropertyChanged();
            }
        }

        public TourDTO()
        {
            StartDate = DateTime.Today;
            EndDate = DateTime.Today.AddDays(7);
            Status = "Активен";
            Name = "";
            Description = "";
            Destination = "";
            Price = 0;
            AvailableSeats = 0;
        }

        public TourDTO(tours tour)
        {
            TourId = tour.tourid;
            Name = tour.name;
            Description = tour.description;
            Price = tour.price;
            StartDate = tour.startdate;
            EndDate = tour.enddate;
            Destination = tour.destination;
            AvailableSeats = tour.availableseats;
            Status = tour.status;
            TransportId = tour.transportid;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public object Clone()
        {
            return new TourDTO
            {
                TourId = this.TourId,
                Name = this.Name,
                Description = this.Description,
                Price = this.Price,
                StartDate = this.StartDate,
                EndDate = this.EndDate,
                Destination = this.Destination,
                AvailableSeats = this.AvailableSeats,
                Status = this.Status,
                TransportId = this.TransportId
            };
        }
    }
}