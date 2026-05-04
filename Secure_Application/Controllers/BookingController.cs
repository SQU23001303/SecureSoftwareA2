using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Secure_Application.Models;

namespace Secure_Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private const string ConnectionString = "Data Source=secure.db";

    [HttpPost("create")]
    public IActionResult CreateBooking(Booking booking)
    {
        if (string.IsNullOrWhiteSpace(booking.CustomerName) ||
            string.IsNullOrWhiteSpace(booking.ServiceType) ||
            string.IsNullOrWhiteSpace(booking.BookingDate))
        {
            return BadRequest("All booking fields are required.");
        }

        if (!DateTime.TryParse(booking.BookingDate, out _))
        {
            return BadRequest("Booking date must be a valid date.");
        }

        try
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Bookings (CustomerName, ServiceType, BookingDate)
                VALUES (@customerName, @serviceType, @bookingDate);
            ";

            command.Parameters.AddWithValue("@customerName", booking.CustomerName);
            command.Parameters.AddWithValue("@serviceType", booking.ServiceType);
            command.Parameters.AddWithValue("@bookingDate", booking.BookingDate);

            command.ExecuteNonQuery();

            return Ok("Booking created securely. Input was validated before storage.");
        }
        catch
        {
            return BadRequest("Booking could not be created.");
        }
    }

    [HttpGet("search")]
    public IActionResult SearchBooking(string customerName)
    {
        if (string.IsNullOrWhiteSpace(customerName))
        {
            return BadRequest("Customer name is required.");
        }

        try
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT Id, CustomerName, ServiceType, BookingDate
                FROM Bookings
                WHERE CustomerName = @customerName;
            ";

            command.Parameters.AddWithValue("@customerName", customerName);

            using var reader = command.ExecuteReader();

            var bookings = new List<object>();

            while (reader.Read())
            {
                bookings.Add(new
                {
                    Id = reader.GetInt32(0),
                    CustomerName = reader.GetString(1),
                    ServiceType = reader.GetString(2),
                    BookingDate = reader.GetString(3)
                });
            }

            if (bookings.Count == 0)
            {
                return Ok("No bookings found.");
            }

            return Ok(bookings);
        }
        catch
        {
            return BadRequest("Search could not be completed.");
        }
    }
}