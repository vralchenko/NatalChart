using System.Reflection;
using System.Text.Json;
using NatalChart.Core.Interfaces;
using NatalChart.Core.Models;

namespace NatalChart.Interpretation;

public class NumerologyService : INumerologyService
{
    private readonly Dictionary<string, Dictionary<string, string>> _lifePathTexts = new();
    private readonly Dictionary<string, Dictionary<string, string>> _birthdayTexts = new();

    public NumerologyService()
    {
        var enData = LoadJson("numerology.json");
        var ruData = LoadJson("numerology-ru.json");
        var deData = LoadJson("numerology-de.json");
        var ukData = LoadJson("numerology-uk.json");

        _lifePathTexts["en"] = enData.GetValueOrDefault("lifePath", new Dictionary<string, string>());
        _lifePathTexts["ru"] = ruData.GetValueOrDefault("lifePath", new Dictionary<string, string>());
        _lifePathTexts["de"] = deData.GetValueOrDefault("lifePath", new Dictionary<string, string>());
        _lifePathTexts["uk"] = ukData.GetValueOrDefault("lifePath", new Dictionary<string, string>());
        _birthdayTexts["en"] = enData.GetValueOrDefault("birthday", new Dictionary<string, string>());
        _birthdayTexts["ru"] = ruData.GetValueOrDefault("birthday", new Dictionary<string, string>());
        _birthdayTexts["de"] = deData.GetValueOrDefault("birthday", new Dictionary<string, string>());
        _birthdayTexts["uk"] = ukData.GetValueOrDefault("birthday", new Dictionary<string, string>());
    }

    public NumerologyResult Calculate(DateTime birthDate, string lang = "en")
    {
        var effectiveLang = _lifePathTexts.ContainsKey(lang) ? lang : "en";

        var lifePathNumber = CalculateLifePathNumber(birthDate);
        var birthdayNumber = ReduceToSingleDigit(birthDate.Day);

        var lifePathDesc = _lifePathTexts[effectiveLang].GetValueOrDefault(lifePathNumber.ToString(), "");
        var birthdayDesc = _birthdayTexts[effectiveLang].GetValueOrDefault(birthdayNumber.ToString(), "");

        return new NumerologyResult
        {
            LifePathNumber = lifePathNumber,
            LifePathDescription = lifePathDesc,
            BirthdayNumber = birthdayNumber,
            BirthdayDescription = birthdayDesc,
        };
    }

    private static int CalculateLifePathNumber(DateTime birthDate)
    {
        var dayReduced = ReduceToSingleDigit(birthDate.Day);
        var monthReduced = ReduceToSingleDigit(birthDate.Month);
        var yearReduced = ReduceToSingleDigit(birthDate.Year);

        return ReduceToSingleDigit(dayReduced + monthReduced + yearReduced);
    }

    private static int ReduceToSingleDigit(int number)
    {
        while (number > 9 && number != 11 && number != 22 && number != 33)
        {
            var sum = 0;
            while (number > 0)
            {
                sum += number % 10;
                number /= 10;
            }
            number = sum;
        }
        return number;
    }

    private static Dictionary<string, Dictionary<string, string>> LoadJson(string fileName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var basePath = Path.GetDirectoryName(assembly.Location) ?? "";
        var filePath = Path.Combine(basePath, "Data", fileName);

        if (!File.Exists(filePath))
            return new Dictionary<string, Dictionary<string, string>>();

        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(json)
               ?? new Dictionary<string, Dictionary<string, string>>();
    }
}
