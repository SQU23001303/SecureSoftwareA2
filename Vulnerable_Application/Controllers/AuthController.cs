using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Vulnerable_Application.Models;

namespace Vulnerable_Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly string _connection = "Data Source=vulnerable_app.db";

    [HttpPost("register")]
    public IActionResult Register(User user)
    {
        using var conn = new SqliteConnection(_connection);
        conn.Open();

        var cmd = conn.CreateCommand();

        cmd.CommandText = $"INSERT INTO Users (Username, Password) VALUES ('{user.Username}', '{user.Password}')";

        cmd.ExecuteNonQuery();

        return Ok("User registered");
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        using var conn = new SqliteConnection(_connection);
        conn.Open();

        var cmd = conn.CreateCommand();

        cmd.CommandText = $"SELECT COUNT(*) FROM Users WHERE Username = '{request.Username}' AND Password = '{request.Password}'";

        var result = Convert.ToInt32(cmd.ExecuteScalar());

        if (result > 0)
            return Ok("Login successful");

        return Unauthorized();
    }
}