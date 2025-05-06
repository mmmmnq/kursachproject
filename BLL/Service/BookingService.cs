using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DTO;
using BLL.Repositories;
using Interfaces.Services;

namespace BLL.Services
{
    public class BookingService : IBookingService
    {
        private readonly BookingRepository _bookingRepository;

        public BookingService(string connectionString)
        {
            _bookingRepository = new BookingRepository(connectionString);
        }

        public async Task<(string TourName, string Destination)> GetTourDetailsAsync(int tourId)
        {
            return await _bookingRepository.GetTourDetailsAsync(tourId);
        }

        public async Task<string> GetUserNameAsync(int userId)
        {
            return await _bookingRepository.GetUserNameAsync(userId);
        }

        public async Task AddBookingAsync(int userId, int tourId, int numberOfSeats, decimal totalPrice)
        {
            await _bookingRepository.AddBookingAsync(userId, tourId, numberOfSeats, totalPrice);
        }

        public async Task<BookingDTO> GetBookingDetailsAsync(int bookingId)
        {
            return await _bookingRepository.GetBookingDetailsAsync(bookingId);
        }
        public async Task<IEnumerable<BookingDTO>> GetAllBookingsAsync()
        {
            return await _bookingRepository.GetAllBookingsAsync();
        }

        public async Task DeleteBookingAsync(int bookingId)
        {
            await _bookingRepository.DeleteBookingAsync(bookingId);
        }

        public async Task<IEnumerable<BookingDTO>> GetUserBookingsAsync(int userId)
        {
            return await _bookingRepository.GetUserBookingsAsync(userId);
        }
        public async Task<bool> HasUserBookedTourAsync(int userId, int tourId)
        {
            return await _bookingRepository.HasUserBookedTourAsync(userId, tourId);
        }
    }
}
