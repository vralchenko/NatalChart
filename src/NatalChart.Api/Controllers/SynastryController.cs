using Microsoft.AspNetCore.Mvc;
using NatalChart.Core.Interfaces;
using NatalChart.Core.Models;
using NatalChart.Infrastructure;

namespace NatalChart.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SynastryController : ControllerBase
{
    private readonly IChartCalculator _chartCalculator;
    private readonly IAspectService _aspectService;
    private readonly ITimeZoneService _timeZoneService;

    public SynastryController(
        IChartCalculator chartCalculator,
        IAspectService aspectService,
        ITimeZoneService timeZoneService)
    {
        _chartCalculator = chartCalculator;
        _aspectService = aspectService;
        _timeZoneService = timeZoneService;
    }

    [HttpPost("calculate")]
    public ActionResult<SynastryResult> Calculate([FromBody] SynastryRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        ConvertToUtc(request.Person1);
        ConvertToUtc(request.Person2);

        var chart1 = _chartCalculator.Calculate(request.Person1);
        var chart2 = _chartCalculator.Calculate(request.Person2);
        var interAspects = _aspectService.CalculateInterAspects(chart1.Planets, chart2.Planets);

        return Ok(new SynastryResult
        {
            Chart1 = chart1,
            Chart2 = chart2,
            InterAspects = interAspects
        });
    }

    private void ConvertToUtc(BirthData birthData)
    {
        if (string.IsNullOrEmpty(birthData.TimeZoneId))
            birthData.TimeZoneId = _timeZoneService.GetTimeZoneId(birthData.Latitude, birthData.Longitude);

        var timeParts = birthData.BirthTime.Split(':');
        var hour = int.Parse(timeParts[0]);
        var minute = timeParts.Length > 1 ? int.Parse(timeParts[1]) : 0;

        var localDateTime = new DateTime(
            birthData.BirthDate.Year, birthData.BirthDate.Month, birthData.BirthDate.Day,
            hour, minute, 0
        );

        var utcDateTime = _timeZoneService.ConvertToUtc(localDateTime, birthData.Latitude, birthData.Longitude);
        birthData.BirthDate = utcDateTime.Date;
        birthData.BirthTime = utcDateTime.ToString("HH:mm");
    }
}

public class SynastryRequest
{
    public BirthData Person1 { get; set; } = new();
    public BirthData Person2 { get; set; } = new();
}
