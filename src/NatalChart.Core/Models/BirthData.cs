using System.ComponentModel.DataAnnotations;
using NatalChart.Core.Enums;

namespace NatalChart.Core.Models;

public class BirthData
{
    [Required]
    public DateTime BirthDate { get; set; }

    [Required]
    public string BirthTime { get; set; } = string.Empty;

    [Range(-90, 90)]
    public double Latitude { get; set; }

    [Range(-180, 180)]
    public double Longitude { get; set; }

    public string TimeZoneId { get; set; } = string.Empty;

    public HouseSystem HouseSystem { get; set; } = HouseSystem.Placidus;
}
