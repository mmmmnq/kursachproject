
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel;


namespace Interfaces.Services
{
    public interface ITourService
    {
        Task<List<TourWithImageDTO>> GetToursAsync();
        Task<TourWithImageDTO> GetTourDetailsAsync(int tourId);
        Task<bool> BookTourAsync(int tourId, int userId);

        Task<bool> AddTourAsync(TourDTO tour);
        Task<bool> UpdateTourAsync(TourDTO tour);
        Task<bool> DeleteTourAsync(int tourId);


    }
}