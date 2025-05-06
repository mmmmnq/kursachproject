using BLL.Repositories;
using DAL;
using DTO;
using Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class TourService : ITourService
    {
        private readonly TourRepository _tourRepository;

        public TourService(string connectionString)
        {
            _tourRepository = new TourRepository(connectionString);
        }

        public async Task<List<TourWithImageDTO>> GetToursAsync()
        {
            // Получаем туры через репозиторий
            return await _tourRepository.GetToursAsync();
        }

        public async Task<TourWithImageDTO> GetTourDetailsAsync(int tourId)
        {
            // Получаем подробности тура через репозиторий
            return await _tourRepository.GetTourDetailsAsync(tourId);
        }

        public async Task<bool> BookTourAsync(int tourId, int userId)
        {
            // Создаем бронирование через репозиторий
            return await _tourRepository.BookTourAsync(tourId, userId);
        }

        public async Task<bool> AddTourAsync(TourDTO tour)
        {
            return await _tourRepository.AddTourAsync(tour);
        }

        public async Task<bool> UpdateTourAsync(TourDTO tour)
        {
            return await _tourRepository.UpdateTourAsync(tour);
        }

        public async Task<bool> DeleteTourAsync(int tourId)
        {
            return await _tourRepository.DeleteTourAsync(tourId);
        }
    }
}
