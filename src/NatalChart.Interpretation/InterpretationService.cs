using System.Reflection;
using System.Text.Json;
using NatalChart.Core.Enums;
using NatalChart.Core.Interfaces;
using NatalChart.Core.Models;

namespace NatalChart.Interpretation;

public class InterpretationService : IInterpretationService
{
    private readonly Dictionary<string, string> _planetInSign;
    private readonly Dictionary<string, string> _planetInHouse;
    private readonly Dictionary<string, string> _aspectTexts;

    public InterpretationService()
    {
        _planetInSign = LoadJson("planet-in-sign.json");
        _planetInHouse = LoadJson("planet-in-house.json");
        _aspectTexts = LoadJson("aspects.json");
    }

    public InterpretationResult GetInterpretations(NatalChartResult chart)
    {
        var result = new InterpretationResult();

        foreach (var planet in chart.Planets)
        {
            if (planet.Body == CelestialBody.Ascendant || planet.Body == CelestialBody.Midheaven)
                continue;

            // Planet in sign
            var signKey = $"{planet.Body}_{planet.Sign}";
            if (_planetInSign.TryGetValue(signKey, out var signText))
            {
                result.PlanetInSign.Add(new InterpretationEntry
                {
                    Key = signKey,
                    Title = $"{planet.Body} in {planet.Sign}",
                    Text = signText
                });
            }

            // Planet in house
            var houseKey = $"{planet.Body}_House{planet.House}";
            if (_planetInHouse.TryGetValue(houseKey, out var houseText))
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

            var text = "";
            if (_aspectTexts.TryGetValue(aspectKey, out var t))
                text = t;
            else if (_aspectTexts.TryGetValue(reverseKey, out var rt))
                text = rt;

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
