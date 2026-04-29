using Microsoft.Data.Sqlite;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

InitialiseDatabase();

app.Run();

void InitialiseDatabase()
{
    using var connection = new SqliteConnection("Data Source=vulnerable_app.db");
    connection.Open();

    var command = connection.CreateCommand();

    command.CommandText =
    @"
    CREATE TABLE IF NOT EXISTS Users (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        Username TEXT NOT NULL,
        Password TEXT NOT NULL
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