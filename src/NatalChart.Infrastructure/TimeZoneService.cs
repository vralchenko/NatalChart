using GeoTimeZone;
using NodaTime;

namespace NatalChart.Infrastructure;

public interface ITimeZoneService
{
    DateTime ConvertToUtc(DateTime localDateTime, double latitude, double longitude);
    string GetTimeZoneId(double latitude, double longitude);
}

public class TimeZoneService : ITimeZoneService
{
    public string GetTimeZoneId(double latitude, double longitude)
    {
        var tzResult = TimeZoneLookup.GetTimeZone(latitude, longitude);
        return tzResult.Result;
    }

    public DateTime ConvertToUtc(DateTime localDateTime, double latitude, double longitude)
    {
        var tzId = GetTimeZoneId(latitude, longitude);
        var tz = DateTimeZoneProviders.Tzdb[tzId];

        var localDate = new LocalDateTime(
            localDateTime.Year,
            localDateTime.Month,
            localDateTime.Day,
            localDateTime.Hour,
            localDateTime.Minute,
            localDateTime.Second
        );

        var zonedDateTime = localDate.InZoneLeniently(tz);
        var instant = zonedDateTime.ToInstant();

        return instant.ToDateTimeUtc();
    }
}
