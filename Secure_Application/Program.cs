using Microsoft.Data.Sqlite;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
    await next();
});

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

CreateDatabase();

app.Run();

void CreateDatabase()
{
    using var connection = new SqliteConnection("Data Source=secure.db");
    connection.Open();

    var command = connection.CreateCommand();

    command.CommandText = @"
        CREATE TABLE IF NOT EXISTS Users (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Username TEXT NOT NULL UNIQUE,
            PasswordHash TEXT NOT NULL
        );

        CREATE TABLE IF NOT EXISTS Bookings (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            CustomerName TEXT NOT NULL,
            ServiceType TEXT NOT NULL,
            BookingDate TEXT NOT NULL
        );
    ";

    command.ExecuteNonQuery();
}