using NatalChart.Core.Enums;

namespace NatalChart.Core.Models;

public class Aspect
{
    public CelestialBody Body1 { get; set; }
    public CelestialBody Body2 { get; set; }
    public AspectType Type { get; set; }
    public double Angle { get; set; }
    public double Orb { get; set; }
    public bool IsApplying { get; set; }
}
