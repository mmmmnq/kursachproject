using System;
namespace kursovaya.Models
{
    public class Tour
    {
        public int TourId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Destination { get; set; }
        public int TransportId { get; set; }
        public int AvailableSeats { get; set; }
        public string Status { get; set; }
    }
}