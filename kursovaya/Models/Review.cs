
namespace kursovaya.Models
{ 
public class Review
{
    public int ReviewId { get; set; }
    public int TourId { get; set; }
    public string ReviewerName { get; set; }
    public string Comment { get; set; }
    public int Rating { get; set; } // Оценка отзыва
    public string Date { get; set; }
}
}