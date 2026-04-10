using Microsoft.AspNetCore.Mvc;
using NatalChart.Core.Interfaces;
using NatalChart.Core.Models;
using NatalChart.Infrastructure;

namespace NatalChart.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransitController : ControllerBase
{
    private readonly IChartCalculator _chartCalculator;
    private readonly IEphemerisService _ephemerisService;
    private readonly IAspectService _aspectService;
    private readonly ITimeZoneService _timeZoneService;

    public TransitController(
        IChartCalculator chartCalculator,
        IEphemerisService ephemerisService,
        IAspectService aspectService,
        ITimeZoneService timeZoneService)
    {
        _chartCalculator = chartCalculator;
        _ephemerisService = ephemerisService;
        _aspectService = aspectService;
        _timeZoneService = timeZoneService;
    }

    [HttpPost("calculate")]
    public ActionResult<TransitResult> Calculate([FromBody] TransitRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Convert natal data to UTC
        if (string.IsNullOrEmpty(request.NatalData.TimeZoneId))
            request.NatalData.TimeZoneId = _timeZoneService.GetTimeZoneId(request.NatalData.Latitude, request.NatalData.Longitude);

        var timeParts = request.NatalData.BirthTime.Split(':');
        var hour = int.Parse(timeParts[0]);
        var minute = timeParts.Length > 1 ? int.Parse(timeParts[1]) : 0;
        var localDateTime = new DateTime(
            request.NatalData.BirthDate.Year, request.NatalData.BirthDate.Month, request.NatalData.BirthDate.Day,
            hour, minute, 0
        );
        var utcDateTime = _timeZoneService.ConvertToUtc(localDateTime, request.NatalData.Latitude, request.NatalData.Longitude);
        request.NatalData.BirthDate = utcDateTime.Date;
        request.NatalData.BirthTime = utcDateTime.ToString("HH:mm");

        var natalChart = _chartCalculator.Calculate(request.NatalData);

        // Calculate transit positions for the given date
        var transitDate = request.TransitDate ?? DateTime.UtcNow;
        var julianDay = _ephemerisService.DateTimeToJulianDay(transitDate);
        var transitPlanets = _ephemerisService.CalculateAllPlanets(julianDay);

        // Calculate aspects between transit planets and natal planets
        var transitAspects = _aspectService.CalculateInterAspects(transitPlanets, natalChart.Planets);

        return Ok(new TransitResult
        {
            NatalChart = natalChart,
            TransitPlanets = transitPlanets,
            TransitAspects = transitAspects
        });
    }
}

public class TransitRequest
{
    public BirthData NatalData { get; set; } = new();
    public DateTime? TransitDate { get; set; }
}
