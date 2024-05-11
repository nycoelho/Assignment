using Assignment.Infrastructure.Api;
using FluentAssertions;

namespace WeatherForecast.UnitTests;

public class ApiTests
{
    private WeatherForecastApi _weatherForecastApi;

    [SetUp]
    public void Setup()
    {
        _weatherForecastApi = new WeatherForecastApi();
    }

    [Test]
    public void GetTemperature_ShouldReturn12_WhenCityIsLisbonAndHourIs12()
    {
        // Arrange
        const string cityName = "Lisbon";
        DateTime time = new(2024, 5, 11, 12, 0, 0);

        // Act
        int temperature = _weatherForecastApi.GetTemperature(cityName, time);

        // Assert
        temperature.Should().Be(12);
    }

    [Test]
    public void GetTemperature_ShouldReturn11_WhenCityIsAmsterdamAndHourIs18()
    {
        // Arrange
        const string cityName = "Amsterdam";
        DateTime time = new(2024, 5, 11, 18, 0, 0);

        // Act
        int temperature = _weatherForecastApi.GetTemperature(cityName, time);

        // Assert
        temperature.Should().Be(11);
    }
}
