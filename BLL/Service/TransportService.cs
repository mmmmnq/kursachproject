using BLL.Repositories;
using DAL;
using DTO;
using Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class TransportService : ITransportService
    {
        private readonly TransportRepository _transportRepository;

        public TransportService(string connectionString)
        {
            _transportRepository = new TransportRepository(connectionString);
        }

        public async Task<List<TransportDTO>> GetAllTransportsAsync()
        {
            return await _transportRepository.GetAllTransportsAsync();
        }

        /// <summary>
        /// Добавляет новый транспорт.
        /// </summary>
        public async Task AddTransportAsync(TransportDTO transport)
        {
            await _transportRepository.AddTransportAsync(transport);
        }

        /// <summary>
        /// Удаляет транспорт по его идентификатору.
        /// </summary>
        public async Task DeleteTransportAsync(int transportId)
        {
            await _transportRepository.DeleteTransportAsync(transportId);
        }
        public async Task<decimal> GetTransportPriceAsync(int tourId)
        {
            // Получаем цену транспорта через репозиторий
            return await _transportRepository.GetTransportPriceAsync(tourId);
        }

        public async Task UpdateTransportAsync(TransportDTO transport)
        {
            await _transportRepository.UpdateTransportAsync(transport);
        }
    }
}
