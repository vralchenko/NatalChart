namespace NatalChart.Core.Models;

public class TransitResult
{
    public NatalChartResult NatalChart { get; set; } = new();
    public List<PlanetPosition> TransitPlanets { get; set; } = new();
    public List<Aspect> TransitAspects { get; set; } = new();
}
