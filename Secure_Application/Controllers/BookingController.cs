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
    public IActionResult CreateBooking([FromBody] Booking booking)
    {
        if (booking == null ||
            string.IsNullOrWhiteSpace(booking.Username) ||
            string.IsNullOrWhiteSpace(booking.CustomerName) ||
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
                INSERT INTO Bookings (Username, CustomerName, ServiceType, BookingDate)
                VALUES (@username, @customerName, @serviceType, @bookingDate);
            ";

            command.Parameters.AddWithValue("@username", booking.Username.Trim());
            command.Parameters.AddWithValue("@customerName", booking.CustomerName.Trim());
            command.Parameters.AddWithValue("@serviceType", booking.ServiceType.Trim());
            command.Parameters.AddWithValue("@bookingDate", booking.BookingDate);

            command.ExecuteNonQuery();

            return Ok("Booking created successfully.");
        }
        catch
        {
            return BadRequest("Booking could not be created.");
        }
    }

    [HttpGet("my-bookings")]
    public IActionResult MyBookings([FromQuery] string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return BadRequest("User must be logged in to view bookings.");
        }

        try
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT Id, Username, CustomerName, ServiceType, BookingDate
                FROM Bookings
                WHERE Username = @username;
            ";

            command.Parameters.AddWithValue("@username", username.Trim());

            using var reader = command.ExecuteReader();

            var bookings = new List<object>();

            while (reader.Read())
            {
                bookings.Add(new
                {
                    Id = reader.GetInt32(0),
                    Username = reader.GetString(1),
                    CustomerName = reader.GetString(2),
                    ServiceType = reader.GetString(3),
                    BookingDate = reader.GetString(4)
                });
            }

            if (bookings.Count == 0)
            {
                return Ok("No bookings found for this user.");
            }

            return Ok(bookings);
        }
        catch
        {
            return BadRequest("Bookings could not be loaded.");
        }
    }
}