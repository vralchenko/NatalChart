using NatalChart.Core.Enums;
using NatalChart.Core.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace NatalChart.Api.Services;

public class PdfExportService
{
    private static readonly Dictionary<string, Dictionary<string, string>> PlanetNames = new()
    {
        ["en"] = new() {
            ["Sun"] = "Sun", ["Moon"] = "Moon", ["Mercury"] = "Mercury", ["Venus"] = "Venus",
            ["Mars"] = "Mars", ["Jupiter"] = "Jupiter", ["Saturn"] = "Saturn", ["Uranus"] = "Uranus",
            ["Neptune"] = "Neptune", ["Pluto"] = "Pluto", ["NorthNode"] = "North Node", ["Chiron"] = "Chiron",
            ["Ascendant"] = "Ascendant", ["Midheaven"] = "Midheaven",
        },
        ["ru"] = new() {
            ["Sun"] = "Солнце", ["Moon"] = "Луна", ["Mercury"] = "Меркурий", ["Venus"] = "Венера",
            ["Mars"] = "Марс", ["Jupiter"] = "Юпитер", ["Saturn"] = "Сатурн", ["Uranus"] = "Уран",
            ["Neptune"] = "Нептун", ["Pluto"] = "Плутон", ["NorthNode"] = "Сев. Узел", ["Chiron"] = "Хирон",
            ["Ascendant"] = "Асцендент", ["Midheaven"] = "Середина Неба",
        },
        ["de"] = new() {
            ["Sun"] = "Sonne", ["Moon"] = "Mond", ["Mercury"] = "Merkur", ["Venus"] = "Venus",
            ["Mars"] = "Mars", ["Jupiter"] = "Jupiter", ["Saturn"] = "Saturn", ["Uranus"] = "Uranus",
            ["Neptune"] = "Neptun", ["Pluto"] = "Pluto", ["NorthNode"] = "Mondknoten", ["Chiron"] = "Chiron",
            ["Ascendant"] = "Aszendent", ["Midheaven"] = "Medium Coeli",
        },
        ["uk"] = new() {
            ["Sun"] = "Сонце", ["Moon"] = "Місяць", ["Mercury"] = "Меркурій", ["Venus"] = "Венера",
            ["Mars"] = "Марс", ["Jupiter"] = "Юпітер", ["Saturn"] = "Сатурн", ["Uranus"] = "Уран",
            ["Neptune"] = "Нептун", ["Pluto"] = "Плутон", ["NorthNode"] = "Пн. Вузол", ["Chiron"] = "Хірон",
            ["Ascendant"] = "Асцендент", ["Midheaven"] = "Середина Неба",
        },
    };

    private static readonly Dictionary<string, Dictionary<string, string>> SignNames = new()
    {
        ["en"] = new() {
            ["Aries"] = "Aries", ["Taurus"] = "Taurus", ["Gemini"] = "Gemini", ["Cancer"] = "Cancer",
            ["Leo"] = "Leo", ["Virgo"] = "Virgo", ["Libra"] = "Libra", ["Scorpio"] = "Scorpio",
            ["Sagittarius"] = "Sagittarius", ["Capricorn"] = "Capricorn", ["Aquarius"] = "Aquarius", ["Pisces"] = "Pisces",
        },
        ["ru"] = new() {
            ["Aries"] = "Овен", ["Taurus"] = "Телец", ["Gemini"] = "Близнецы", ["Cancer"] = "Рак",
            ["Leo"] = "Лев", ["Virgo"] = "Дева", ["Libra"] = "Весы", ["Scorpio"] = "Скорпион",
            ["Sagittarius"] = "Стрелец", ["Capricorn"] = "Козерог", ["Aquarius"] = "Водолей", ["Pisces"] = "Рыбы",
        },
        ["de"] = new() {
            ["Aries"] = "Widder", ["Taurus"] = "Stier", ["Gemini"] = "Zwillinge", ["Cancer"] = "Krebs",
            ["Leo"] = "Löwe", ["Virgo"] = "Jungfrau", ["Libra"] = "Waage", ["Scorpio"] = "Skorpion",
            ["Sagittarius"] = "Schütze", ["Capricorn"] = "Steinbock", ["Aquarius"] = "Wassermann", ["Pisces"] = "Fische",
        },
        ["uk"] = new() {
            ["Aries"] = "Овен", ["Taurus"] = "Телець", ["Gemini"] = "Близнюки", ["Cancer"] = "Рак",
            ["Leo"] = "Лев", ["Virgo"] = "Діва", ["Libra"] = "Терези", ["Scorpio"] = "Скорпіон",
            ["Sagittarius"] = "Стрілець", ["Capricorn"] = "Козеріг", ["Aquarius"] = "Водолій", ["Pisces"] = "Риби",
        },
    };

    private static readonly Dictionary<string, Dictionary<string, string>> AspectNames = new()
    {
        ["en"] = new() {
            ["Conjunction"] = "Conjunction", ["Opposition"] = "Opposition", ["Trine"] = "Trine",
            ["Square"] = "Square", ["Sextile"] = "Sextile", ["Quincunx"] = "Quincunx",
        },
        ["ru"] = new() {
            ["Conjunction"] = "Соединение", ["Opposition"] = "Оппозиция", ["Trine"] = "Трин",
            ["Square"] = "Квадратура", ["Sextile"] = "Секстиль", ["Quincunx"] = "Квиконс",
        },
        ["de"] = new() {
            ["Conjunction"] = "Konjunktion", ["Opposition"] = "Opposition", ["Trine"] = "Trigon",
            ["Square"] = "Quadrat", ["Sextile"] = "Sextil", ["Quincunx"] = "Quincunx",
        },
        ["uk"] = new() {
            ["Conjunction"] = "Сполучення", ["Opposition"] = "Опозиція", ["Trine"] = "Трін",
            ["Square"] = "Квадратура", ["Sextile"] = "Секстиль", ["Quincunx"] = "Квіконс",
        },
    };

    private static readonly Dictionary<string, Dictionary<string, string>> Labels = new()
    {
        ["en"] = new() {
            ["title"] = "Natal Chart Report",
            ["birthData"] = "Birth Data",
            ["date"] = "Date",
            ["time"] = "Time",
            ["place"] = "Place",
            ["planetPositions"] = "Planet Positions",
            ["planet"] = "Planet",
            ["sign"] = "Sign",
            ["position"] = "Position",
            ["house"] = "House",
            ["retrograde"] = "Retrograde",
            ["aspects"] = "Aspects",
            ["planet1"] = "Planet 1",
            ["aspect"] = "Aspect",
            ["planet2"] = "Planet 2",
            ["interpretation"] = "Interpretation",
            ["planetsInSigns"] = "Planets in Signs",
            ["planetsInHouses"] = "Planets in Houses",
            ["numerology"] = "Numerology",
            ["lifePathNumber"] = "Life Path Number",
            ["birthdayNumber"] = "Birthday Number",
            ["yes"] = "Yes",
            ["no"] = "No",
            ["generatedOn"] = "Generated on",
        },
        ["ru"] = new() {
            ["title"] = "Отчёт натальной карты",
            ["birthData"] = "Данные рождения",
            ["date"] = "Дата",
            ["time"] = "Время",
            ["place"] = "Место",
            ["planetPositions"] = "Позиции планет",
            ["planet"] = "Планета",
            ["sign"] = "Знак",
            ["position"] = "Позиция",
            ["house"] = "Дом",
            ["retrograde"] = "Ретроградность",
            ["aspects"] = "Аспекты",
            ["planet1"] = "Планета 1",
            ["aspect"] = "Аспект",
            ["planet2"] = "Планета 2",
            ["interpretation"] = "Интерпретация",
            ["planetsInSigns"] = "Планеты в знаках",
            ["planetsInHouses"] = "Планеты в домах",
            ["numerology"] = "Нумерология",
            ["lifePathNumber"] = "Число жизненного пути",
            ["birthdayNumber"] = "Число дня рождения",
            ["yes"] = "Да",
            ["no"] = "Нет",
            ["generatedOn"] = "Сгенерировано",
        },
        ["de"] = new() {
            ["title"] = "Geburtshoroskop-Bericht",
            ["birthData"] = "Geburtsdaten",
            ["date"] = "Datum",
            ["time"] = "Uhrzeit",
            ["place"] = "Ort",
            ["planetPositions"] = "Planetenpositionen",
            ["planet"] = "Planet",
            ["sign"] = "Zeichen",
            ["position"] = "Position",
            ["house"] = "Haus",
            ["retrograde"] = "Rückläufig",
            ["aspects"] = "Aspekte",
            ["planet1"] = "Planet 1",
            ["aspect"] = "Aspekt",
            ["planet2"] = "Planet 2",
            ["interpretation"] = "Deutung",
            ["planetsInSigns"] = "Planeten in Zeichen",
            ["planetsInHouses"] = "Planeten in Häusern",
            ["numerology"] = "Numerologie",
            ["lifePathNumber"] = "Lebenszahl",
            ["birthdayNumber"] = "Geburtstagszahl",
            ["yes"] = "Ja",
            ["no"] = "Nein",
            ["generatedOn"] = "Erstellt am",
        },
        ["uk"] = new() {
            ["title"] = "Звіт натальної карти",
            ["birthData"] = "Дані народження",
            ["date"] = "Дата",
            ["time"] = "Час",
            ["place"] = "Місце",
            ["planetPositions"] = "Позиції планет",
            ["planet"] = "Планета",
            ["sign"] = "Знак",
            ["position"] = "Позиція",
            ["house"] = "Дім",
            ["retrograde"] = "Ретроградність",
            ["aspects"] = "Аспекти",
            ["planet1"] = "Планета 1",
            ["aspect"] = "Аспект",
            ["planet2"] = "Планета 2",
            ["interpretation"] = "Інтерпретація",
            ["planetsInSigns"] = "Планети у знаках",
            ["planetsInHouses"] = "Планети у домах",
            ["numerology"] = "Нумерологія",
            ["lifePathNumber"] = "Число життєвого шляху",
            ["birthdayNumber"] = "Число дня народження",
            ["yes"] = "Так",
            ["no"] = "Ні",
            ["generatedOn"] = "Згенеровано",
        },
    };

    private static readonly Dictionary<string, string> SignSymbols = new()
    {
        ["Aries"] = "\u2648", ["Taurus"] = "\u2649", ["Gemini"] = "\u264A", ["Cancer"] = "\u264B",
        ["Leo"] = "\u264C", ["Virgo"] = "\u264D", ["Libra"] = "\u264E", ["Scorpio"] = "\u264F",
        ["Sagittarius"] = "\u2650", ["Capricorn"] = "\u2651", ["Aquarius"] = "\u2652", ["Pisces"] = "\u2653",
    };

    private static readonly Dictionary<string, string> AspectColorHex = new()
    {
        ["Conjunction"] = "#FBBF24",
        ["Opposition"] = "#EF4444",
        ["Trine"] = "#22C55E",
        ["Square"] = "#F97316",
        ["Sextile"] = "#3B82F6",
        ["Quincunx"] = "#9CA3AF",
    };

    private string L(string lang, string key) =>
        Labels.TryGetValue(lang, out var d) && d.TryGetValue(key, out var v) ? v : Labels["en"][key];

    private string Planet(string lang, string key) =>
        PlanetNames.TryGetValue(lang, out var d) && d.TryGetValue(key, out var v) ? v : key;

    private string Sign(string lang, string key) =>
        SignNames.TryGetValue(lang, out var d) && d.TryGetValue(key, out var v) ? v : key;

    private string Aspect(string lang, string key) =>
        AspectNames.TryGetValue(lang, out var d) && d.TryGetValue(key, out var v) ? v : key;

    public byte[] GeneratePdf(
        BirthData birthData,
        string locationName,
        NatalChartResult chart,
        InterpretationResult interpretations,
        NumerologyResult numerology,
        string lang)
    {
        var effectiveLang = Labels.ContainsKey(lang) ? lang : "en";

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.MarginHorizontal(40);
                page.MarginVertical(35);
                page.DefaultTextStyle(x => x.FontSize(10).FontColor(Colors.Grey.Darken3));

                page.Header().Element(c => ComposeHeader(c, effectiveLang, birthData, locationName));

                page.Content().Element(c => ComposeContent(c, effectiveLang, chart, interpretations, numerology));

                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span("NatalChart — ");
                    text.CurrentPageNumber();
                    text.Span(" / ");
                    text.TotalPages();
                });
            });
        });

        return document.GeneratePdf();
    }

    private void ComposeHeader(IContainer container, string lang, BirthData birthData, string locationName)
    {
        container.Column(col =>
        {
            col.Item().Text(L(lang, "title")).FontSize(20).Bold().FontColor(Colors.Purple.Darken2);
            col.Item().PaddingTop(5).Text($"{L(lang, "generatedOn")} {DateTime.UtcNow:yyyy-MM-dd}").FontSize(8).FontColor(Colors.Grey.Medium);
            col.Item().PaddingTop(8).LineHorizontal(1).LineColor(Colors.Purple.Lighten3);
            col.Item().PaddingTop(8).Row(row =>
            {
                row.RelativeItem().Column(c =>
                {
                    c.Item().Text($"{L(lang, "date")}: {birthData.BirthDate:yyyy-MM-dd}");
                    c.Item().Text($"{L(lang, "time")}: {birthData.BirthTime}");
                });
                row.RelativeItem().Column(c =>
                {
                    c.Item().Text($"{L(lang, "place")}: {locationName}");
                });
            });
            col.Item().PaddingTop(8).LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten2);
        });
    }

    private void ComposeContent(IContainer container, string lang,
        NatalChartResult chart, InterpretationResult interpretations, NumerologyResult numerology)
    {
        container.PaddingTop(10).Column(col =>
        {
            // Planet Positions Table
            col.Item().Text(L(lang, "planetPositions")).FontSize(14).Bold().FontColor(Colors.Purple.Darken1);
            col.Item().PaddingTop(5).Table(table =>
            {
                table.ColumnsDefinition(c =>
                {
                    c.RelativeColumn(2.5f);
                    c.RelativeColumn(2f);
                    c.RelativeColumn(2f);
                    c.RelativeColumn(1f);
                    c.RelativeColumn(1.5f);
                });

                table.Header(h =>
                {
                    h.Cell().Background(Colors.Purple.Lighten4).Padding(4).Text(L(lang, "planet")).Bold();
                    h.Cell().Background(Colors.Purple.Lighten4).Padding(4).Text(L(lang, "sign")).Bold();
                    h.Cell().Background(Colors.Purple.Lighten4).Padding(4).Text(L(lang, "position")).Bold();
                    h.Cell().Background(Colors.Purple.Lighten4).Padding(4).Text(L(lang, "house")).Bold();
                    h.Cell().Background(Colors.Purple.Lighten4).Padding(4).Text(L(lang, "retrograde")).Bold();
                });

                foreach (var p in chart.Planets)
                {
                    var bodyStr = p.Body.ToString();
                    var signStr = p.Sign.ToString();
                    var bgColor = chart.Planets.ToList().IndexOf(p) % 2 == 0 ? Colors.White : Colors.Grey.Lighten4;

                    table.Cell().Background(bgColor).Padding(3).Text(Planet(lang, bodyStr));
                    table.Cell().Background(bgColor).Padding(3).Text($"{SignSymbols.GetValueOrDefault(signStr, "")} {Sign(lang, signStr)}");
                    table.Cell().Background(bgColor).Padding(3).Text($"{p.SignDegree}\u00B0 {p.SignMinute:D2}' {p.SignSecond:D2}\"");
                    table.Cell().Background(bgColor).Padding(3).Text(p.House > 0 ? p.House.ToString() : "-");
                    table.Cell().Background(bgColor).Padding(3).Text(p.IsRetrograde ? L(lang, "yes") : "");
                }
            });

            col.Item().PaddingTop(15);

            // Aspects Table
            col.Item().Text(L(lang, "aspects")).FontSize(14).Bold().FontColor(Colors.Purple.Darken1);
            col.Item().PaddingTop(5).Table(table =>
            {
                table.ColumnsDefinition(c =>
                {
                    c.RelativeColumn(2.5f);
                    c.RelativeColumn(2f);
                    c.RelativeColumn(2.5f);
                });

                table.Header(h =>
                {
                    h.Cell().Background(Colors.Purple.Lighten4).Padding(4).Text(L(lang, "planet1")).Bold();
                    h.Cell().Background(Colors.Purple.Lighten4).Padding(4).Text(L(lang, "aspect")).Bold();
                    h.Cell().Background(Colors.Purple.Lighten4).Padding(4).Text(L(lang, "planet2")).Bold();
                });

                foreach (var a in chart.Aspects)
                {
                    var typeStr = a.Type.ToString();
                    var colorHex = AspectColorHex.GetValueOrDefault(typeStr, "#9CA3AF");
                    var bgColor = Color.FromHex(colorHex + "14");

                    table.Cell().Background(bgColor).Padding(3).Text(Planet(lang, a.Body1.ToString()));
                    table.Cell().Background(bgColor).Padding(3).Text(Aspect(lang, typeStr)).FontColor(Color.FromHex(colorHex));
                    table.Cell().Background(bgColor).Padding(3).Text(Planet(lang, a.Body2.ToString()));
                }
            });

            col.Item().PaddingTop(15);

            // Interpretation Summary
            if (!string.IsNullOrEmpty(interpretations.Summary))
            {
                col.Item().Text(L(lang, "interpretation")).FontSize(14).Bold().FontColor(Colors.Purple.Darken1);
                col.Item().PaddingTop(5).Text(interpretations.Summary);
                col.Item().PaddingTop(10);
            }

            // Planets in Signs
            if (interpretations.PlanetInSign.Count > 0)
            {
                col.Item().Text(L(lang, "planetsInSigns")).FontSize(13).Bold().FontColor(Colors.Purple.Medium);
                foreach (var entry in interpretations.PlanetInSign)
                {
                    col.Item().PaddingTop(5).Text(entry.Title).Bold().FontSize(10);
                    col.Item().Text(entry.Text).FontSize(9).FontColor(Colors.Grey.Darken2);
                }
                col.Item().PaddingTop(10);
            }

            // Planets in Houses
            if (interpretations.PlanetInHouse.Count > 0)
            {
                col.Item().Text(L(lang, "planetsInHouses")).FontSize(13).Bold().FontColor(Colors.Purple.Medium);
                foreach (var entry in interpretations.PlanetInHouse)
                {
                    col.Item().PaddingTop(5).Text(entry.Title).Bold().FontSize(10);
                    col.Item().Text(entry.Text).FontSize(9).FontColor(Colors.Grey.Darken2);
                }
                col.Item().PaddingTop(10);
            }

            // Aspects Interpretations
            if (interpretations.Aspects.Count > 0)
            {
                col.Item().Text(L(lang, "aspects")).FontSize(13).Bold().FontColor(Colors.Purple.Medium);
                foreach (var entry in interpretations.Aspects)
                {
                    col.Item().PaddingTop(5).Text(entry.Title).Bold().FontSize(10);
                    col.Item().Text(entry.Text).FontSize(9).FontColor(Colors.Grey.Darken2);
                }
                col.Item().PaddingTop(10);
            }

            // Numerology
            col.Item().Text(L(lang, "numerology")).FontSize(14).Bold().FontColor(Colors.Purple.Darken1);
            col.Item().PaddingTop(5).Row(row =>
            {
                row.RelativeItem().Border(1).BorderColor(Colors.Purple.Lighten3).Padding(10).Column(c =>
                {
                    c.Item().Text($"{L(lang, "lifePathNumber")}: {numerology.LifePathNumber}").Bold().FontSize(12).FontColor(Colors.Purple.Darken2);
                    c.Item().PaddingTop(4).Text(numerology.LifePathDescription).FontSize(9);
                });
                row.ConstantItem(10);
                row.RelativeItem().Border(1).BorderColor(Colors.Purple.Lighten3).Padding(10).Column(c =>
                {
                    c.Item().Text($"{L(lang, "birthdayNumber")}: {numerology.BirthdayNumber}").Bold().FontSize(12).FontColor(Colors.Purple.Darken2);
                    c.Item().PaddingTop(4).Text(numerology.BirthdayDescription).FontSize(9);
                });
            });
        });
    }
}
