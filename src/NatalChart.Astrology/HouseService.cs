using NatalChart.Core.Enums;
using NatalChart.Core.Interfaces;
using NatalChart.Core.Models;
using NatalChart.Astrology.Constants;
using SwissEphNet;

namespace NatalChart.Astrology;

public class HouseService : IHouseService
{
    public List<HouseCusp> CalculateHouses(double julianDay, double latitude, double longitude, HouseSystem system)
    {
        var cusps = new double[13]; // index 1-12
        var ascmc = new double[10]; // ASC, MC, etc.

        var houseSystemCode = SwissEphConstants.HouseSystemCodes.GetValueOrDefault(system, 'P');

        using var swissEph = new SwissEph();
        var ret = swissEph.swe_houses(julianDay, latitude, longitude, houseSystemCode, cusps, ascmc);
        if (ret < 0)
            throw new InvalidOperationException($"SwissEph house calculation failed for system {system}");

        var houses = new List<HouseCusp>();
        for (int i = 1; i <= 12; i++)
        {
            var cuspLon = cusps[i];
            var normalizedLon = ((cuspLon % 360) + 360) % 360;
            var signIndex = (int)(normalizedLon / 30.0);
            var sign = (ZodiacSign)(signIndex + 1);
            var degreeInSign = normalizedLon - signIndex * 30.0;
            var degree = (int)degreeInSign;
            var minutesFull = (degreeInSign - degree) * 60.0;
            var minutes = (int)minutesFull;

            houses.Add(new HouseCusp
            {
                HouseNumber = i,
                Longitude = normalizedLon,
                Sign = sign,
                Degree = degree,
                Minute = minutes
            });
        }

        return houses;
    }
}
