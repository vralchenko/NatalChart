using NatalChart.Core.Enums;

namespace NatalChart.Core.Models;

public class PlanetPosition
{
    public CelestialBody Body { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public double Speed { get; set; }
    public bool IsRetrograde => Speed < 0;
    public ZodiacSign Sign { get; set; }
    public int SignDegree { get; set; }
    public int SignMinute { get; set; }
    public int SignSecond { get; set; }
    public int House { get; set; }
}
