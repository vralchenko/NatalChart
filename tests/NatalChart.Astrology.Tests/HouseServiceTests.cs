using NatalChart.Astrology;
using NatalChart.Core.Enums;

namespace NatalChart.Astrology.Tests;

public class HouseServiceTests
{
    private readonly HouseService _service;

    public HouseServiceTests()
    {
        _service = new HouseService();
    }

    [Fact]
    public void CalculateHouses_Placidus_Returns12Houses()
    {
        using var ephService = new EphemerisService();
        var jd = ephService.DateTimeToJulianDay(new DateTime(2000, 1, 1, 12, 0, 0, DateTimeKind.Utc));

        var houses = _service.CalculateHouses(jd, 51.4769, -0.0005, HouseSystem.Placidus);

        Assert.Equal(12, houses.Count);
        for (int i = 0; i < 12; i++)
        {
            Assert.Equal(i + 1, houses[i].HouseNumber);
            Assert.InRange(houses[i].Longitude, 0, 360);
            Assert.True(Enum.IsDefined(typeof(ZodiacSign), houses[i].Sign));
        }
    }

    [Fact]
    public void CalculateHouses_Greenwich_AscendantInAries()
    {
        // For Jan 1 2000, 12:00 UTC at Greenwich, Ascendant should be in Aries
        using var ephService = new EphemerisService();
        var jd = ephService.DateTimeToJulianDay(new DateTime(2000, 1, 1, 12, 0, 0, DateTimeKind.Utc));

        var houses = _service.CalculateHouses(jd, 51.4769, -0.0005, HouseSystem.Placidus);
        var ascendant = houses[0]; // House 1 cusp = ASC

        Assert.Equal(ZodiacSign.Aries, ascendant.Sign);
    }

    [Fact]
    public void CalculateHouses_DifferentSystems_ProduceDifferentResults()
    {
        using var ephService = new EphemerisService();
        var jd = ephService.DateTimeToJulianDay(new DateTime(2000, 1, 1, 12, 0, 0, DateTimeKind.Utc));

        var placidus = _service.CalculateHouses(jd, 51.4769, -0.0005, HouseSystem.Placidus);
        var koch = _service.CalculateHouses(jd, 51.4769, -0.0005, HouseSystem.Koch);

        // ASC should be the same, but intermediate houses differ
        Assert.Equal(placidus[0].Longitude, koch[0].Longitude, 1);
        // Some intermediate cusps should differ
        var hasDifference = false;
        for (int i = 1; i < 12; i++)
        {
            if (Math.Abs(placidus[i].Longitude - koch[i].Longitude) > 0.1)
            {
                hasDifference = true;
                break;
            }
        }
        Assert.True(hasDifference);
    }
}
