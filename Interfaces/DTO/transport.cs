using System;
using System.ComponentModel;
using DomainModel;

namespace DTO
{
    public class TransportDTO : INotifyPropertyChanged
    {
        private int transportId;
        private string type;
        private int capacity;
        private string company;
        private decimal price;

        public event PropertyChangedEventHandler PropertyChanged;

        public int TransportId
        {
            get => transportId;
            set
            {
                transportId = value;
                OnPropertyChanged(nameof(TransportId));
            }
        }

        public string Type
        {
            get => type;
            set
            {
                type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        public int Capacity
        {
            get => capacity;
            set
            {
                capacity = value;
                OnPropertyChanged(nameof(Capacity));
            }
        }

        public string Company
        {
            get => company;
            set
            {
                company = value;
                OnPropertyChanged(nameof(Company));
            }
        }

        public decimal Price
        {
            get => price;
            set
            {
                price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TransportDTO() { }

        public TransportDTO(transport transport)
        {
            TransportId = transport.transportid;
            Type = transport.type;
            Capacity = transport.capacity;
            Company = transport.company;
            Price = transport.price;
        }
    }
}