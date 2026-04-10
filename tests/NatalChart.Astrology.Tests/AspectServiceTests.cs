using NatalChart.Astrology;
using NatalChart.Core.Enums;
using NatalChart.Core.Models;

namespace NatalChart.Astrology.Tests;

public class AspectServiceTests
{
    private readonly AspectService _service;

    public AspectServiceTests()
    {
        _service = new AspectService();
    }

    [Fact]
    public void CalculateAspects_Conjunction_Detected()
    {
        var planets = new List<PlanetPosition>
        {
            new() { Body = CelestialBody.Sun, Longitude = 100.0, Speed = 1.0 },
            new() { Body = CelestialBody.Moon, Longitude = 103.0, Speed = 12.0 }
        };

        var aspects = _service.CalculateAspects(planets);

        Assert.Single(aspects);
        Assert.Equal(AspectType.Conjunction, aspects[0].Type);
        Assert.Equal(3.0, aspects[0].Orb, 1);
    }

    [Fact]
    public void CalculateAspects_Opposition_Detected()
    {
        var planets = new List<PlanetPosition>
        {
            new() { Body = CelestialBody.Sun, Longitude = 10.0, Speed = 1.0 },
            new() { Body = CelestialBody.Saturn, Longitude = 188.0, Speed = 0.1 }
        };

        var aspects = _service.CalculateAspects(planets);

        Assert.Single(aspects);
        Assert.Equal(AspectType.Opposition, aspects[0].Type);
    }

    [Fact]
    public void CalculateAspects_Trine_Detected()
    {
        var planets = new List<PlanetPosition>
        {
            new() { Body = CelestialBody.Venus, Longitude = 30.0, Speed = 1.2 },
            new() { Body = CelestialBody.Jupiter, Longitude = 150.0, Speed = 0.1 }
        };

        var aspects = _service.CalculateAspects(planets);

        Assert.Single(aspects);
        Assert.Equal(AspectType.Trine, aspects[0].Type);
    }

    [Fact]
    public void CalculateAspects_NoAspect_WhenOutOfOrb()
    {
        var planets = new List<PlanetPosition>
        {
            new() { Body = CelestialBody.Mars, Longitude = 10.0, Speed = 0.5 },
            new() { Body = CelestialBody.Saturn, Longitude = 55.0, Speed = 0.1 } // 45 degrees, no major aspect
        };

        var aspects = _service.CalculateAspects(planets);

        Assert.Empty(aspects);
    }

    [Fact]
    public void CalculateAspects_WrapAround360_Conjunction()
    {
        var planets = new List<PlanetPosition>
        {
            new() { Body = CelestialBody.Sun, Longitude = 358.0, Speed = 1.0 },
            new() { Body = CelestialBody.Moon, Longitude = 2.0, Speed = 12.0 }
        };

        var aspects = _service.CalculateAspects(planets);

        Assert.Single(aspects);
        Assert.Equal(AspectType.Conjunction, aspects[0].Type);
    }

    [Fact]
    public void CalculateInterAspects_DetectsAspectsBetweenTwoSets()
    {
        var planets1 = new List<PlanetPosition>
        {
            new() { Body = CelestialBody.Sun, Longitude = 100.0, Speed = 1.0 }
        };
        var planets2 = new List<PlanetPosition>
        {
            new() { Body = CelestialBody.Moon, Longitude = 280.0, Speed = 12.0 }
        };

        var aspects = _service.CalculateInterAspects(planets1, planets2);

        Assert.Single(aspects);
        Assert.Equal(AspectType.Opposition, aspects[0].Type);
    }
}
