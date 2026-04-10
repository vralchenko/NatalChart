using NatalChart.Core.Enums;
using NatalChart.Core.Models;

namespace NatalChart.Core.Interfaces;

public interface IHouseService
{
    List<HouseCusp> CalculateHouses(double julianDay, double latitude, double longitude, HouseSystem system);
}
