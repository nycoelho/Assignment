﻿using Assignment.Application.Common.Interfaces;
using Assignment.Application.Countries.Queries.GetCountries;
using Caliburn.Micro;
using MediatR;

namespace Assignment.UI;
internal class WeatherForecastViewModel : Screen
{
    private readonly ISender _sender;
    private readonly IWeatherForecastApi _weatherForecastApi;

    private IList<CountryDto> _countries;
    public IList<CountryDto> Countries
    {
        get => _countries;
        set
        {
            _countries = value;
            NotifyOfPropertyChange(() => Countries);
        }
    }

    private CountryDto _selectedCountry;
    public CountryDto SelectedCountry
    {
        get => _selectedCountry;
        set
        {
            _selectedCountry = value;
            NotifyOfPropertyChange(() => SelectedCountry);
        }
    }

    private CityDto _selectedCity;
    public CityDto SelectedCity
    {
        get => _selectedCity;
        set
        {
            _selectedCity = value;
            if (_selectedCity is not null)
            {
                Temperature = _weatherForecastApi.GetTemperature(_selectedCity.Name, DateTime.Now);
            }
            else
            {
                Temperature = null;
            }
            NotifyOfPropertyChange(() => SelectedCity);
        }
    }

    private int? _temperature;
    public int? Temperature
    {
        get => _temperature;
        set
        {
            _temperature = value;
            NotifyOfPropertyChange(() => Temperature);
        }
    }

    public WeatherForecastViewModel(ISender sender, IWeatherForecastApi weatherForecastApi)
    {
        _sender = sender;
        _weatherForecastApi = weatherForecastApi;
        Initialize();
    }

    private async void Initialize()
    {
        await RefreshCountries();
    }

    private async Task RefreshCountries()
    {
        var selectedCountryId = SelectedCountry?.Id;

        Countries = await _sender.Send(new GetCountriesQuery());

        if (selectedCountryId.HasValue && selectedCountryId.Value > 0)
        {
            SelectedCountry = Countries.FirstOrDefault(c => c.Id == selectedCountryId.Value);
        }
    }
}
