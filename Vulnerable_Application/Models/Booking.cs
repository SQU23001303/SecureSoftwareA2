namespace Vulnerable_Application.Models;

public class Booking
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = "";
    public string ServiceType { get; set; } = "";
    public string BookingDate { get; set; } = "";
}