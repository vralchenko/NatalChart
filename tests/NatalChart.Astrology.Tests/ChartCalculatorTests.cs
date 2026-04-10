using Moq;
using NatalChart.Astrology;
using NatalChart.Core.Enums;
using NatalChart.Core.Interfaces;
using NatalChart.Core.Models;

namespace NatalChart.Astrology.Tests;

public class ChartCalculatorTests
{
    [Fact]
    public void Calculate_GoldenChart_ReturnsCompleteResult()
    {
        // Integration test with real services
        using var ephService = new EphemerisService();
        var houseService = new HouseService();
        var aspectService = new AspectService();
        var calculator = new ChartCalculator(ephService, houseService, aspectService);

        var birthData = new BirthData
        {
            BirthDate = new DateTime(2000, 1, 1),
            BirthTime = "12:00",
            Latitude = 51.4769,
            Longitude = -0.0005,
            HouseSystem = HouseSystem.Placidus
        };

        var result = calculator.Calculate(birthData);

        // Should have 12 planet bodies + ASC + MC = 14
        Assert.Equal(14, result.Planets.Count);
        Assert.Equal(12, result.Houses.Count);
        Assert.True(result.Aspects.Count > 0);

        // Verify Sun is in Capricorn
        var sun = result.Planets.First(p => p.Body == CelestialBody.Sun);
        Assert.Equal(ZodiacSign.Capricorn, sun.Sign);

        // Verify ASC and MC are present
        Assert.Contains(result.Planets, p => p.Body == CelestialBody.Ascendant);
        Assert.Contains(result.Planets, p => p.Body == CelestialBody.Midheaven);

        // Verify all planets are assigned to houses
        var planetsWithHouses = result.Planets.Where(p =>
            p.Body != CelestialBody.Ascendant && p.Body != CelestialBody.Midheaven);
        foreach (var planet in planetsWithHouses)
        {
            Assert.InRange(planet.House, 1, 12);
        }
    }

    [Fact]
    public void Calculate_WithMocks_CallsAllServices()
    {
        var mockEph = new Mock<IEphemerisService>();
        var mockHouse = new Mock<IHouseService>();
        var mockAspect = new Mock<IAspectService>();

        var planets = new List<PlanetPosition>
        {
            new() { Body = CelestialBody.Sun, Longitude = 280, Sign = ZodiacSign.Capricorn, Speed = 1 },
            new() { Body = CelestialBody.Moon, Longitude = 100, Sign = ZodiacSign.Cancer, Speed = 12 }
        };

        var houses = Enumerable.Range(1, 12).Select(i => new HouseCusp
        {
            HouseNumber = i,
            Longitude = (i - 1) * 30.0,
            Sign = (ZodiacSign)i,
            Degree = 0
        }).ToList();

        mockEph.Setup(s => s.DateTimeToJulianDay(It.IsAny<DateTime>())).Returns(2451545.0);
        mockEph.Setup(s => s.CalculateAllPlanets(It.IsAny<double>())).Returns(planets);
        mockHouse.Setup(s => s.CalculateHouses(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<HouseSystem>())).Returns(houses);
        mockAspect.Setup(s => s.CalculateAspects(It.IsAny<List<PlanetPosition>>())).Returns(new List<Aspect>());

        var calculator = new ChartCalculator(mockEph.Object, mockHouse.Object, mockAspect.Object);

        var birthData = new BirthData
        {
            BirthDate = new DateTime(2000, 1, 1),
            BirthTime = "12:00",
            Latitude = 51.4769,
            Longitude = -0.0005
        };

        var result = calculator.Calculate(birthData);

        mockEph.Verify(s => s.DateTimeToJulianDay(It.IsAny<DateTime>()), Times.Once);
        mockEph.Verify(s => s.CalculateAllPlanets(It.IsAny<double>()), Times.Once);
        mockHouse.Verify(s => s.CalculateHouses(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<HouseSystem>()), Times.Once);
        mockAspect.Verify(s => s.CalculateAspects(It.IsAny<List<PlanetPosition>>()), Times.Once);
    }
}
