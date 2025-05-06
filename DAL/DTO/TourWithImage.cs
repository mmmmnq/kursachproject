using System;

public class TourWithImage
{
    public int TourId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Destination { get; set; }
    public int AvailableSeats { get; set; }
    public string Status { get; set; }
    public string ImagePath { get; set; }
    public decimal TransportPrice { get; set; }
}
