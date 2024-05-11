using Assignment.Application.Common.Interfaces;

namespace Assignment.Infrastructure.Api;

public class WeatherForecastApi : IWeatherForecastApi
{
    public int GetTemperature(string cityName, DateTime time)
    {
        // mock API: simple algorithm to create a random temperature with a seed
        // based on the city name and hour of the day
        int cityHash = cityName.GetHashCode();
        int hourOfDay = time.Hour;
        int seed = cityHash ^ hourOfDay;
        Random rng = new(seed);
        return rng.Next(10, 30);
    }
}
