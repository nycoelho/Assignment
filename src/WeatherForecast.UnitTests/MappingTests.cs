using System.Reflection;
using System.Runtime.CompilerServices;
using Assignment.Application.Common.Interfaces;
using Assignment.Application.Countries.Queries.GetCountries;
using Assignment.Domain.Entities;
using AutoMapper;

namespace WeatherForecast.UnitTests;

public class MappingTests
{
    private IConfigurationProvider _configurationProvider;
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        _configurationProvider = new MapperConfiguration(config =>
            config.AddMaps(Assembly.GetAssembly(typeof(IApplicationDbContext))));

        _mapper = _configurationProvider.CreateMapper();
    }

    [Test]
    public void IConfigurationProvider_ShouldHaveValidConfiguration()
    {
        // Assert
        _configurationProvider.AssertConfigurationIsValid();
    }

    [Test]
    [TestCase(typeof(Country), typeof(CountryDto))]
    [TestCase(typeof(City), typeof(CityDto))]
    public void IMapper_ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
    {
        // Arrange
        var instance = GetInstanceOf(source);

        // Act
        _mapper.Map(instance, source, destination);
    }

    private object GetInstanceOf(Type type)
    {
        if (type.GetConstructor(Type.EmptyTypes) is not null)
        {
            return Activator.CreateInstance(type)!;
        }

        // Type without parameterless constructor
        return RuntimeHelpers.GetUninitializedObject(type);
    }
}
