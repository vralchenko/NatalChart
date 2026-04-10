namespace NatalChart.Core.Models;

public class NatalChartResult
{
    public List<PlanetPosition> Planets { get; set; } = new();
    public List<HouseCusp> Houses { get; set; } = new();
    public List<Aspect> Aspects { get; set; } = new();
}
