using Models;

namespace WeatherAPI;

public class ForecastService
{
    private readonly IDatabaseService _databaseService;
    
    public ForecastService(IDatabaseService? databaseService = null)
    {
        databaseService ??= new DatabaseService();
        _databaseService = databaseService;
    }
    
    public WeatherForecast[] GetForecast()
    {
        var summaries = _databaseService.GetSummaries();
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateTime.Now.AddDays(index),
                    Random.Shared.Next(-20, 55),
                    Random.Shared.Next(0, 12),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        
        _databaseService.SaveForecasts(forecast);
        return forecast;
    }
}