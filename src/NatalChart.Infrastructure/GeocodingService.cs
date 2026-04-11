using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using NatalChart.Core.Interfaces;
using NatalChart.Core.Models;

namespace NatalChart.Infrastructure;

public class GeocodingService : IGeocodingService
{
    private readonly HttpClient _httpClient;

    public GeocodingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("NatalChart/1.0");
    }

    public async Task<List<GeocodingResult>> SearchAsync(string query, string lang = "en")
    {
        var url = $"https://nominatim.openstreetmap.org/search?format=json&q={Uri.EscapeDataString(query)}&addressdetails=1&limit=10&dedupe=1&accept-language={Uri.EscapeDataString(lang)}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var results = JsonSerializer.Deserialize<List<NominatimResult>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return results?.Select(r => new GeocodingResult
            {
                DisplayName = r.DisplayName,
                Latitude = double.TryParse(r.Lat, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var lat) ? lat : 0,
                Longitude = double.TryParse(r.Lon, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var lon) ? lon : 0
            })
            .DistinctBy(r => r.DisplayName)
            .Take(5)
            .ToList() ?? new List<GeocodingResult>();
    }

    private class NominatimResult
    {
        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("lat")]
        public string Lat { get; set; } = string.Empty;

        [JsonPropertyName("lon")]
        public string Lon { get; set; } = string.Empty;
    }
}
