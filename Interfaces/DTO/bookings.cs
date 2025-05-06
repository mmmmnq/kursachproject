using System;
using DomainModel;

namespace DTO
{
    public class BookingDTO
    {
        public int BookingId { get; set; }
        public int? UserId { get; set; }

        public string UserName { get; set; }

   
        public int? TourId { get; set; }
        public DateTime BookingDate { get; set; }
        public int NumberOfSeats { get; set; }
        public decimal TotalPrice { get; set; }

        public BookingDTO() { }

        public BookingDTO(bookings booking,string userName)
        {
            BookingId = booking.bookingid;
            UserId = booking.userid;
            TourId = booking.tourid;
            UserName = userName;
            BookingDate = booking.bookingdate;
            NumberOfSeats = booking.numberofseats;
            TotalPrice = booking.totalprice;
        }
    }
}
