using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Secure_Application.Models;
using System.Security.Cryptography;
using System.Text;

namespace Secure_Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private const string ConnectionString = "Data Source=secure.db";

    [HttpPost("register")]
    public IActionResult Register(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Username and password are required.");
        }

        if (request.Username.Length < 3)
        {
            return BadRequest("Username must be at least 3 characters.");
        }

        if (request.Password.Length < 8)
        {
            return BadRequest("Password must be at least 8 characters.");
        }

        try
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            string passwordHash = HashPassword(request.Password);

            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Users (Username, PasswordHash)
                VALUES (@username, @passwordHash);
            ";

            command.Parameters.AddWithValue("@username", request.Username.Trim());
            command.Parameters.AddWithValue("@passwordHash", passwordHash);

            command.ExecuteNonQuery();

            return Ok("User registered securely.");
        }
        catch
        {
            return BadRequest("Registration failed.");
        }
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Username and password are required.");
        }

        try
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT PasswordHash 
                FROM Users
                WHERE Username = @username;
            ";

            command.Parameters.AddWithValue("@username", request.Username.Trim());

            var storedHash = command.ExecuteScalar()?.ToString();

            if (storedHash == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            string enteredHash = HashPassword(request.Password);

            if (storedHash != enteredHash)
            {
                return Unauthorized("Invalid username or password.");
            }

            return Ok("Login successful.");
        }
        catch
        {
            return BadRequest("Login failed.");
        }
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}