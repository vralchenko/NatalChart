using NatalChart.Core.Enums;
using NatalChart.Core.Interfaces;
using NatalChart.Core.Models;

namespace NatalChart.Astrology;

public class ChartCalculator : IChartCalculator
{
    private readonly IEphemerisService _ephemerisService;
    private readonly IHouseService _houseService;
    private readonly IAspectService _aspectService;

    public ChartCalculator(IEphemerisService ephemerisService, IHouseService houseService, IAspectService aspectService)
    {
        _ephemerisService = ephemerisService;
        _houseService = houseService;
        _aspectService = aspectService;
    }

    public NatalChartResult Calculate(BirthData birthData)
    {
        // Parse birth time
        var timeParts = birthData.BirthTime.Split(':');
        var hour = int.Parse(timeParts[0]);
        var minute = timeParts.Length > 1 ? int.Parse(timeParts[1]) : 0;

        var utcDateTime = new DateTime(
            birthData.BirthDate.Year,
            birthData.BirthDate.Month,
            birthData.BirthDate.Day,
            hour, minute, 0,
            DateTimeKind.Utc
        );

        var julianDay = _ephemerisService.DateTimeToJulianDay(utcDateTime);

        // Calculate planet positions
        var planets = _ephemerisService.CalculateAllPlanets(julianDay);

        // Calculate house cusps
        var houses = _houseService.CalculateHouses(julianDay, birthData.Latitude, birthData.Longitude, birthData.HouseSystem);

        // Add Ascendant and Midheaven as planet positions
        if (houses.Count >= 10)
        {
            var ascLon = houses[0].Longitude; // House 1 cusp = Ascendant
            planets.Add(CreatePointPosition(CelestialBody.Ascendant, ascLon));

            // Midheaven = House 10 cusp
            var mcLon = houses[9].Longitude;
            planets.Add(CreatePointPosition(CelestialBody.Midheaven, mcLon));
        }

        // Assign planets to houses
        AssignPlanetsToHouses(planets, houses);

        // Calculate aspects
        var aspects = _aspectService.CalculateAspects(planets);

        return new NatalChartResult
        {
            Planets = planets,
            Houses = houses,
            Aspects = aspects
        };
    }

    private static void AssignPlanetsToHouses(List<PlanetPosition> planets, List<HouseCusp> houses)
    {
        foreach (var planet in planets)
        {
            if (planet.Body == CelestialBody.Ascendant || planet.Body == CelestialBody.Midheaven)
                continue;

            planet.House = DetermineHouse(planet.Longitude, houses);
        }
    }

    private static int DetermineHouse(double longitude, List<HouseCusp> houses)
    {
        for (int i = 0; i < 12; i++)
        {
            var currentCusp = houses[i].Longitude;
            var nextCusp = houses[(i + 1) % 12].Longitude;

            if (nextCusp < currentCusp) // crosses 0 degrees
            {
                if (longitude >= currentCusp || longitude < nextCusp)
                    return houses[i].HouseNumber;
            }
            else
            {
                if (longitude >= currentCusp && longitude < nextCusp)
                    return houses[i].HouseNumber;
            }
        }

        return 1; // fallback
    }

    private static PlanetPosition CreatePointPosition(CelestialBody body, double longitude)
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
            Latitude = 0,
            Speed = 0,
            Sign = sign,
            SignDegree = degree,
            SignMinute = minutes,
            SignSecond = seconds
        };
    }
}
