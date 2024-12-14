using System.Collections.Generic;
using System.Threading.Tasks;

public interface ITourService
{
    Task<List<TourWithImage>> GetToursAsync();
    Task<TourWithImage> GetTourDetailsAsync(int tourId);
    Task<bool> BookTourAsync(int tourId, int userId);
}
