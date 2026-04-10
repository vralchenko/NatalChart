using NatalChart.Core.Enums;
using NatalChart.Core.Models;
using NatalChart.Interpretation;

namespace NatalChart.Interpretation.Tests;

public class InterpretationServiceTests
{
    private readonly InterpretationService _service;

    public InterpretationServiceTests()
    {
        _service = new InterpretationService();
    }

    [Fact]
    public void GetInterpretations_WithSunInAries_ReturnsPlanetInSignEntry()
    {
        var chart = new NatalChartResult
        {
            Planets = new List<PlanetPosition>
            {
                new() { Body = CelestialBody.Sun, Sign = ZodiacSign.Aries, House = 1, Longitude = 15 }
            },
            Houses = new List<HouseCusp>(),
            Aspects = new List<Aspect>()
        };

        var result = _service.GetInterpretations(chart);

        Assert.NotEmpty(result.PlanetInSign);
        Assert.Contains(result.PlanetInSign, e => e.Key == "Sun_Aries");
    }

    [Fact]
    public void GetInterpretations_SkipsAscendantAndMidheaven()
    {
        var chart = new NatalChartResult
        {
            Planets = new List<PlanetPosition>
            {
                new() { Body = CelestialBody.Ascendant, Sign = ZodiacSign.Aries, Longitude = 15 },
                new() { Body = CelestialBody.Midheaven, Sign = ZodiacSign.Capricorn, Longitude = 280 }
            },
            Houses = new List<HouseCusp>(),
            Aspects = new List<Aspect>()
        };

        var result = _service.GetInterpretations(chart);

        Assert.Empty(result.PlanetInSign);
        Assert.Empty(result.PlanetInHouse);
    }

    [Fact]
    public void GetInterpretations_EmptyChart_ReturnsEmptyResult()
    {
        var chart = new NatalChartResult();
        var result = _service.GetInterpretations(chart);

        Assert.Empty(result.PlanetInSign);
        Assert.Empty(result.PlanetInHouse);
        Assert.Empty(result.Aspects);
    }
}
