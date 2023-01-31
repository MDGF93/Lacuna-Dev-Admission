using System.Data.SQLite;
using Lacuna_Dev_Admission.Requests;
using Serilog;

namespace Lacuna_Dev_Admission.Repositories;

public class UserRepository
{
    private static readonly string currentDirectory = Directory.GetCurrentDirectory();
    private static readonly string databasePath = currentDirectory + "\\sqlite.db";
    private static readonly string _connectionString =
        $"Data Source={databasePath}";
    
    public void Save(CreateUserRequest createUserRequest)
    {
        //Create table if it doesn't exist
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            var command = new SQLiteCommand(connection);
            command.CommandText =
                "CREATE TABLE IF NOT EXISTS users (id INTEGER PRIMARY KEY, username TEXT, password TEXT, email TEXT)";
            command.ExecuteNonQuery();
        }

        if (Exists(createUserRequest.UserName))
        {
            Log.Information($"User {createUserRequest.UserName} already exists.");
            return;
        }

        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            var command = new SQLiteCommand(connection);
            command.CommandText = "INSERT INTO users (username, password, email) VALUES (@username, @password, @email)";
            command.Parameters.AddWithValue("@username", createUserRequest.UserName);
            command.Parameters.AddWithValue("@password", createUserRequest.Password);
            command.Parameters.AddWithValue("@email", createUserRequest.Email);
            command.ExecuteNonQuery();
        }
    }

    public bool Exists(string username)
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            var command = new SQLiteCommand(connection);
            command.CommandText = "SELECT * FROM users WHERE username = @username";
            command.Parameters.AddWithValue("@username", username);
            var reader = command.ExecuteReader();
            return reader.HasRows;
        }
    }

    public void UpdateAccessToken(string username, string accessToken)
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            var command = new SQLiteCommand(connection);
            command.CommandText = "UPDATE users SET access_token = @access_token WHERE username = @username";
            command.Parameters.AddWithValue("@access_token", accessToken);
            command.Parameters.AddWithValue("@username", username);
            command.ExecuteNonQuery();
        }
    }
}