using System.Diagnostics.CodeAnalysis;
using MySqlConnector;
using Dapper;
using Models;

namespace WeatherAPI;

public class DatabaseService : IDatabaseService
{   
    public string[] GetSummaries()
    {
        using var connection = GetConnection();
        return connection.Query<string>("SELECT summary FROM Summaries ORDER BY summary").ToArray();
    }

    public void SaveForecasts(WeatherForecast[] forecasts)
    {
        using var connection = GetConnection();
        using var transaction = connection.BeginTransaction();

        foreach (var forecast in forecasts) 
        {
            connection.Execute(@"
                INSERT INTO Forecasts (`date`, temperatureC, summary, wind_speed) VALUES (@Date, @TemperatureC, @Summary, @WindSpeed)
                    ON DUPLICATE KEY UPDATE temperatureC = @TemperatureC, summary = @Summary, wind_speed = @WindSpeed
            ", forecast, transaction);
        }
        transaction.Commit();
    }

    private MySqlConnection GetConnection()
    {
        var connection = new MySqlConnection("Server=mariadb;Database=forecasts;Uid=myuser;Pwd=mypassword;");
        connection.Open();
        return connection;
    }
}