using System.Reflection;
using System.Text.Json;
using NatalChart.Core.Enums;
using NatalChart.Core.Interfaces;
using NatalChart.Core.Models;

namespace NatalChart.Interpretation;

public class InterpretationService : IInterpretationService
{
    private readonly Dictionary<string, Dictionary<string, string>> _planetInSign = new();
    private readonly Dictionary<string, Dictionary<string, string>> _planetInHouse = new();
    private readonly Dictionary<string, Dictionary<string, string>> _aspectTexts = new();

    public InterpretationService()
    {
        _planetInSign["en"] = LoadJson("planet-in-sign.json");
        _planetInSign["ru"] = LoadJson("planet-in-sign-ru.json");
        _planetInSign["de"] = LoadJson("planet-in-sign-de.json");
        _planetInSign["uk"] = LoadJson("planet-in-sign-uk.json");
        _planetInHouse["en"] = LoadJson("planet-in-house.json");
        _planetInHouse["ru"] = LoadJson("planet-in-house-ru.json");
        _planetInHouse["de"] = LoadJson("planet-in-house-de.json");
        _planetInHouse["uk"] = LoadJson("planet-in-house-uk.json");
        _aspectTexts["en"] = LoadJson("aspects.json");
        _aspectTexts["ru"] = LoadJson("aspects-ru.json");
        _aspectTexts["de"] = LoadJson("aspects-de.json");
        _aspectTexts["uk"] = LoadJson("aspects-uk.json");
    }

    public InterpretationResult GetInterpretations(NatalChartResult chart, string lang = "en")
    {
        var effectiveLang = _planetInSign.ContainsKey(lang) ? lang : "en";
        var signData = _planetInSign[effectiveLang];
        var signFallback = _planetInSign["en"];
        var houseData = _planetInHouse[effectiveLang];
        var houseFallback = _planetInHouse["en"];
        var aspectData = _aspectTexts[effectiveLang];
        var aspectFallback = _aspectTexts["en"];

        string? Lookup(Dictionary<string, string> primary, Dictionary<string, string> fallback, string key)
        {
            if (primary.TryGetValue(key, out var v) && !string.IsNullOrWhiteSpace(v)) return v;
            if (fallback.TryGetValue(key, out var f) && !string.IsNullOrWhiteSpace(f)) return f;
            return null;
        }

        var result = new InterpretationResult();

        foreach (var planet in chart.Planets)
        {
            if (planet.Body == CelestialBody.Ascendant || planet.Body == CelestialBody.Midheaven)
                continue;

            var signKey = $"{planet.Body}_{planet.Sign}";
            var signText = Lookup(signData, signFallback, signKey);
            if (signText != null)
            {
                result.PlanetInSign.Add(new InterpretationEntry
                {
                    Key = signKey,
                    Title = $"{planet.Body} in {planet.Sign}",
                    Text = signText
                });
            }

            var houseKey = $"{planet.Body}_House{planet.House}";
            var houseText = Lookup(houseData, houseFallback, houseKey);
            if (houseText != null)
            {
                result.PlanetInHouse.Add(new InterpretationEntry
                {
                    Key = houseKey,
                    Title = $"{planet.Body} in House {planet.House}",
                    Text = houseText
                });
            }
        }

        foreach (var aspect in chart.Aspects)
        {
            var aspectKey = $"{aspect.Body1}_{aspect.Type}_{aspect.Body2}";
            var reverseKey = $"{aspect.Body2}_{aspect.Type}_{aspect.Body1}";

            var text = Lookup(aspectData, aspectFallback, aspectKey)
                    ?? Lookup(aspectData, aspectFallback, reverseKey);

            if (!string.IsNullOrEmpty(text))
            {
                result.Aspects.Add(new InterpretationEntry
                {
                    Key = aspectKey,
                    Title = $"{aspect.Body1} {aspect.Type} {aspect.Body2}",
                    Text = text
                });
            }
        }

        return result;
    }

    private static Dictionary<string, string> LoadJson(string fileName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var basePath = Path.GetDirectoryName(assembly.Location) ?? "";
        var filePath = Path.Combine(basePath, "Data", fileName);

        if (!File.Exists(filePath))
            return new Dictionary<string, string>();

        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
    }
}
