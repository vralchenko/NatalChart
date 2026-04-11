using NatalChart.Core.Models;
using NatalChart.Infrastructure;

namespace NatalChart.Api.Helpers;

public static class BirthDataHelper
{
    public static void ResolveTimezoneAndConvertToUtc(BirthData birthData, ITimeZoneService timeZoneService)
    {
        if (string.IsNullOrEmpty(birthData.TimeZoneId))
        {
            birthData.TimeZoneId = timeZoneService.GetTimeZoneId(birthData.Latitude, birthData.Longitude);
        }

        var timeParts = birthData.BirthTime.Split(':');
        var hour = int.Parse(timeParts[0]);
        var minute = timeParts.Length > 1 ? int.Parse(timeParts[1]) : 0;

        var localDateTime = new DateTime(
            birthData.BirthDate.Year,
            birthData.BirthDate.Month,
            birthData.BirthDate.Day,
            hour, minute, 0
        );

        var utcDateTime = timeZoneService.ConvertToUtc(localDateTime, birthData.Latitude, birthData.Longitude);

        birthData.BirthDate = utcDateTime.Date;
        birthData.BirthTime = utcDateTime.ToString("HH:mm");
    }
}
