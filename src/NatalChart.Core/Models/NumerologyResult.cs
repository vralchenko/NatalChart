namespace NatalChart.Core.Models;

public class NumerologyResult
{
    public int LifePathNumber { get; set; }
    public string LifePathDescription { get; set; } = string.Empty;
    public int BirthdayNumber { get; set; }
    public string BirthdayDescription { get; set; } = string.Empty;
}

public class NumerologyRequest
{
    public DateTime BirthDate { get; set; }
}
