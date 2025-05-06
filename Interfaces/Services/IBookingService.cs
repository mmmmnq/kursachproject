using DTO;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Interfaces.Services
{
    public interface IBookingService
    {
        Task<(string TourName, string Destination)> GetTourDetailsAsync(int tourId);
        Task<string> GetUserNameAsync(int userId);
        Task AddBookingAsync(int userId, int tourId, int numberOfSeats, decimal totalPrice);
        Task<BookingDTO> GetBookingDetailsAsync(int bookingId);
        Task<IEnumerable<BookingDTO>> GetUserBookingsAsync(int userId);

        Task<IEnumerable<BookingDTO>> GetAllBookingsAsync();
        Task DeleteBookingAsync(int bookingId);
    }
}