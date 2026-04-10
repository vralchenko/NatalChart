namespace NatalChart.Core.Models;

public class SynastryResult
{
    public NatalChartResult Chart1 { get; set; } = new();
    public NatalChartResult Chart2 { get; set; } = new();
    public List<Aspect> InterAspects { get; set; } = new();
}
