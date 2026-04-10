using NatalChart.Core.Enums;

namespace NatalChart.Core.Models;

public class HouseCusp
{
    public int HouseNumber { get; set; }
    public double Longitude { get; set; }
    public ZodiacSign Sign { get; set; }
    public int Degree { get; set; }
    public int Minute { get; set; }
}
