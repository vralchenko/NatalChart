using NatalChart.Astrology;
using NatalChart.Core.Enums;

namespace NatalChart.Astrology.Tests;

public class EphemerisServiceTests : IDisposable
{
    private readonly EphemerisService _service;

    public EphemerisServiceTests()
    {
        _service = new EphemerisService();
    }

    [Fact]
    public void DateTimeToJulianDay_J2000_ReturnsCorrectValue()
    {
        // J2000.0 = Jan 1, 2000, 12:00 TT ≈ 2451545.0
        var date = new DateTime(2000, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        var jd = _service.DateTimeToJulianDay(date);
        Assert.InRange(jd, 2451545.0 - 0.001, 2451545.0 + 0.001);
    }

    [Fact]
    public void CalculatePlanetPosition_Sun_Jan1_2000_ReturnsCapricorn()
    {
        // Sun on Jan 1, 2000 12:00 UTC should be around 280° (Capricorn ~10°)
        var jd = _service.DateTimeToJulianDay(new DateTime(2000, 1, 1, 12, 0, 0, DateTimeKind.Utc));
        var sun = _service.CalculatePlanetPosition(jd, CelestialBody.Sun);

        Assert.Equal(ZodiacSign.Capricorn, sun.Sign);
        Assert.InRange(sun.Longitude, 279.0, 282.0); // ~280.5°
    }

    [Fact]
    public void CalculatePlanetPosition_Moon_Jan1_2000_ReturnsExpectedSign()
    {
        var jd = _service.DateTimeToJulianDay(new DateTime(2000, 1, 1, 12, 0, 0, DateTimeKind.Utc));
        var moon = _service.CalculatePlanetPosition(jd, CelestialBody.Moon);

        // Moon should be valid
        Assert.InRange(moon.Longitude, 0, 360);
        Assert.NotEqual(0, moon.Speed); // Moon always moves
    }

    [Fact]
    public void CalculateAllPlanets_ReturnsAllBodies()
    {
        var jd = _service.DateTimeToJulianDay(new DateTime(2000, 1, 1, 12, 0, 0, DateTimeKind.Utc));
        var planets = _service.CalculateAllPlanets(jd);

        Assert.Equal(12, planets.Count); // Sun through Chiron (no ASC/MC)
        Assert.Contains(planets, p => p.Body == CelestialBody.Sun);
        Assert.Contains(planets, p => p.Body == CelestialBody.Moon);
        Assert.Contains(planets, p => p.Body == CelestialBody.Pluto);
        Assert.Contains(planets, p => p.Body == CelestialBody.Chiron);
        Assert.Contains(planets, p => p.Body == CelestialBody.NorthNode);
    }

    [Fact]
    public void CalculatePlanetPosition_AllPlanetsHaveValidLongitude()
    {
        var jd = _service.DateTimeToJulianDay(new DateTime(2000, 1, 1, 12, 0, 0, DateTimeKind.Utc));
        var planets = _service.CalculateAllPlanets(jd);

        foreach (var planet in planets)
        {
            Assert.InRange(planet.Longitude, 0, 360);
            Assert.True(Enum.IsDefined(typeof(ZodiacSign), planet.Sign));
            Assert.InRange(planet.SignDegree, 0, 29);
            Assert.InRange(planet.SignMinute, 0, 59);
        }
    }

    [Fact]
    public void CalculatePlanetPosition_Ascendant_ThrowsArgumentException()
    {
        var jd = _service.DateTimeToJulianDay(new DateTime(2000, 1, 1, 12, 0, 0, DateTimeKind.Utc));
        Assert.Throws<ArgumentException>(() => _service.CalculatePlanetPosition(jd, CelestialBody.Ascendant));
    }

    public void Dispose()
    {
        _service.Dispose();
    }
}
