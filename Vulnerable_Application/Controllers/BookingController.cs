using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Vulnerable_Application.Models;

namespace Vulnerable_Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly string _connection = "Data Source=vulnerable_app.db";

    [HttpPost("create")]
    public IActionResult Create(Booking booking)
    {
        using var conn = new SqliteConnection(_connection);
        conn.Open();

        var cmd = conn.CreateCommand();

        cmd.CommandText =
            $"INSERT INTO Bookings (CustomerName, ServiceType, BookingDate) VALUES ('{booking.CustomerName}', '{booking.ServiceType}', '{booking.BookingDate}')";

        cmd.ExecuteNonQuery();

        return Ok("Booking created");
    }

    [HttpGet("search")]
    public IActionResult Search(string customerName)
    {
        using var conn = new SqliteConnection(_connection);
        conn.Open();

        var cmd = conn.CreateCommand();

        cmd.CommandText =
            $"SELECT * FROM Bookings WHERE CustomerName = '{customerName}'";

        using var reader = cmd.ExecuteReader();

        var list = new List<Booking>();

        while (reader.Read())
        {
            list.Add(new Booking
            {
                Id = reader.GetInt32(0),
                CustomerName = reader.GetString(1),
                ServiceType = reader.GetString(2),
                BookingDate = reader.GetString(3)
            });
        }

        return Ok(list);
    }
}