using NatalChart.Core.Models;

namespace NatalChart.Core.Interfaces;

public interface IGeocodingService
{
    Task<List<GeocodingResult>> SearchAsync(string query);
}
