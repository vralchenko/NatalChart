using NatalChart.Core.Enums;
using NatalChart.Core.Models;

namespace NatalChart.Core.Interfaces;

public interface IEphemerisService
{
    PlanetPosition CalculatePlanetPosition(double julianDay, CelestialBody body);
    List<PlanetPosition> CalculateAllPlanets(double julianDay);
    double DateTimeToJulianDay(DateTime utcDateTime);
}
