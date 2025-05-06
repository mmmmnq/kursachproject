
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel;

namespace Interfaces.Services
{
    public interface ITransportService
    {
        /// <summary>
        /// Получает цену на транспорт для указанного тура.
        /// </summary>
        /// <param name="tourId">Идентификатор тура.</param>
        /// <returns>Цена на транспорт.</returns>
        Task<decimal> GetTransportPriceAsync(int tourId);
    }
}
