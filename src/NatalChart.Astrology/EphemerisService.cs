using System.Reflection;
using NatalChart.Core.Enums;
using NatalChart.Core.Interfaces;
using NatalChart.Core.Models;
using NatalChart.Astrology.Constants;
using SwissEphNet;

namespace NatalChart.Astrology;

public class EphemerisService : IEphemerisService, IDisposable
{
    private readonly SwissEph _swissEph;

    public EphemerisService()
    {
        _swissEph = new SwissEph();
        var assemblyDir = Path.GetDirectoryName(typeof(EphemerisService).Assembly.Location)!;
        var ephePath = Path.Combine(assemblyDir, "Data", "ephe");
        _swissEph.swe_set_ephe_path(ephePath);
        _swissEph.OnLoadFile += (sender, args) =>
        {
            var filePath = args.FileName.Replace('\\', Path.DirectorySeparatorChar);
            if (File.Exists(filePath))
            {
                args.File = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
        };
    }

    public double DateTimeToJulianDay(DateTime utcDateTime)
    {
        return _swissEph.swe_julday(
            utcDateTime.Year,
            utcDateTime.Month,
            utcDateTime.Day,
            utcDateTime.Hour + utcDateTime.Minute / 60.0 + utcDateTime.Second / 3600.0,
            SwissEph.SE_GREG_CAL
        );
    }

    public PlanetPosition CalculatePlanetPosition(double julianDay, CelestialBody body)
    {
        if (body == CelestialBody.Ascendant || body == CelestialBody.Midheaven)
            throw new ArgumentException($"Use IHouseService for {body}");

        if (!SwissEphConstants.PlanetIds.TryGetValue(body, out var planetId))
            throw new ArgumentException($"Unknown celestial body: {body}");

        var result = new double[6];
        var errorMessage = "";

        var flags = SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_SPEED;

        var ret = _swissEph.swe_calc_ut(julianDay, planetId, flags, result, ref errorMessage);
        if (ret < 0)
            throw new InvalidOperationException($"SwissEph error for {body}: {errorMessage}");

        var longitude = result[0];
        var latitude = result[1];
        var speed = result[3];

        return CreatePlanetPosition(body, longitude, latitude, speed);
    }

    public List<PlanetPosition> CalculateAllPlanets(double julianDay)
    {
        var planets = new List<PlanetPosition>();
        var bodies = new[]
        {
            CelestialBody.Sun, CelestialBody.Moon, CelestialBody.Mercury,
            CelestialBody.Venus, CelestialBody.Mars, CelestialBody.Jupiter,
            CelestialBody.Saturn, CelestialBody.Uranus, CelestialBody.Neptune,
            CelestialBody.Pluto, CelestialBody.NorthNode, CelestialBody.Chiron
        };

        foreach (var body in bodies)
        {
            planets.Add(CalculatePlanetPosition(julianDay, body));
        }

        return planets;
    }

    private static PlanetPosition CreatePlanetPosition(CelestialBody body, double longitude, double latitude, double speed)
    {
        var normalizedLon = ((longitude % 360) + 360) % 360;
        var signIndex = (int)(normalizedLon / 30.0);
        var sign = (ZodiacSign)(signIndex + 1);
        var degreeInSign = normalizedLon - signIndex * 30.0;
        var degree = (int)degreeInSign;
        var minutesFull = (degreeInSign - degree) * 60.0;
        var minutes = (int)minutesFull;
        var seconds = (int)((minutesFull - minutes) * 60.0);

        return new PlanetPosition
        {
            Body = body,
            Longitude = normalizedLon,
            Latitude = latitude,
            Speed = speed,
            Sign = sign,
            SignDegree = degree,
            SignMinute = minutes,
            SignSecond = seconds
        };
    }

    public void Dispose()
    {
        _swissEph?.Dispose();
    }
}
