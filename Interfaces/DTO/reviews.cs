using System;
using DomainModel;

namespace DTO
{
    public class ReviewDTO
    {
        public int ReviewId { get; set; }
        public int? UserId { get; set; }
        public int TourId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string UserName { get; set; }
        public DateTime ReviewDate { get; set; }

        public ReviewDTO() { }

        public ReviewDTO(reviews review)
        {
            ReviewId = review.reviewid;
            UserId = review.userid;
            TourId = review.tourid;
            Rating = review.rating;
            Comment = review.comment;
            UserName = review.UserName;
            ReviewDate = review.reviewdate;
        }
    }
}
