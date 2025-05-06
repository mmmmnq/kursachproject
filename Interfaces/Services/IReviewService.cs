
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel;

namespace Interfaces.Services
{
    public interface IReviewService
    {
        Task<List<ReviewDTO>> GetReviewsByTourIdAsync(int tourId);
        Task AddReviewAsync(ReviewDTO review);
        Task UpdateReviewAsync(ReviewDTO review);
        Task DeleteReviewAsync(int reviewId);
    }
}
