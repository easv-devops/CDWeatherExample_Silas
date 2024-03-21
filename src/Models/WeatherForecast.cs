namespace Models;

public record WeatherForecast(DateTime Date, int TemperatureC, int WindSpeed, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}