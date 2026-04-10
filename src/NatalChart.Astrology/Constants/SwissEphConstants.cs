using NatalChart.Core.Enums;
using SwissEphNet;

namespace NatalChart.Astrology.Constants;

public static class SwissEphConstants
{
    public static readonly Dictionary<CelestialBody, int> PlanetIds = new()
    {
        { CelestialBody.Sun, SwissEph.SE_SUN },
        { CelestialBody.Moon, SwissEph.SE_MOON },
        { CelestialBody.Mercury, SwissEph.SE_MERCURY },
        { CelestialBody.Venus, SwissEph.SE_VENUS },
        { CelestialBody.Mars, SwissEph.SE_MARS },
        { CelestialBody.Jupiter, SwissEph.SE_JUPITER },
        { CelestialBody.Saturn, SwissEph.SE_SATURN },
        { CelestialBody.Uranus, SwissEph.SE_URANUS },
        { CelestialBody.Neptune, SwissEph.SE_NEPTUNE },
        { CelestialBody.Pluto, SwissEph.SE_PLUTO },
        { CelestialBody.NorthNode, SwissEph.SE_TRUE_NODE },
        { CelestialBody.Chiron, SwissEph.SE_CHIRON }
    };

    public static readonly Dictionary<HouseSystem, char> HouseSystemCodes = new()
    {
        { HouseSystem.Placidus, 'P' },
        { HouseSystem.Koch, 'K' },
        { HouseSystem.Equal, 'E' },
        { HouseSystem.WholeSign, 'W' },
        { HouseSystem.Campanus, 'C' },
        { HouseSystem.Regiomontanus, 'R' }
    };

    // Orbs: [AspectType] = (luminaryOrb, otherOrb)
    // Luminaries = Sun, Moon
    public static readonly Dictionary<AspectType, (double LuminaryOrb, double OtherOrb)> Orbs = new()
    {
        { AspectType.Conjunction, (10.0, 8.0) },
        { AspectType.Opposition, (10.0, 8.0) },
        { AspectType.Trine, (8.0, 6.0) },
        { AspectType.Square, (8.0, 6.0) },
        { AspectType.Sextile, (6.0, 4.0) },
        { AspectType.Quincunx, (3.0, 2.0) }
    };

    public static readonly Dictionary<AspectType, double> AspectAngles = new()
    {
        { AspectType.Conjunction, 0.0 },
        { AspectType.Opposition, 180.0 },
        { AspectType.Trine, 120.0 },
        { AspectType.Square, 90.0 },
        { AspectType.Sextile, 60.0 },
        { AspectType.Quincunx, 150.0 }
    };

    public static bool IsLuminary(CelestialBody body) =>
        body == CelestialBody.Sun || body == CelestialBody.Moon;
}
