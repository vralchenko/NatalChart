namespace NatalChart.Core.Models;

public class InterpretationResult
{
    public List<InterpretationEntry> PlanetInSign { get; set; } = new();
    public List<InterpretationEntry> PlanetInHouse { get; set; } = new();
    public List<InterpretationEntry> Aspects { get; set; } = new();
}

public class InterpretationEntry
{
    public string Key { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}
