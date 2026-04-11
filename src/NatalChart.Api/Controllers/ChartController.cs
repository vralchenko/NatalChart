using Microsoft.AspNetCore.Mvc;
using NatalChart.Core.Interfaces;
using NatalChart.Core.Models;
using NatalChart.Infrastructure;

namespace NatalChart.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChartController : ControllerBase
{
    private readonly IChartCalculator _chartCalculator;
    private readonly IInterpretationService _interpretationService;
    private readonly ITimeZoneService _timeZoneService;

    public ChartController(
        IChartCalculator chartCalculator,
        IInterpretationService interpretationService,
        ITimeZoneService timeZoneService)
    {
        _chartCalculator = chartCalculator;
        _interpretationService = interpretationService;
        _timeZoneService = timeZoneService;
    }

    [HttpPost("calculate")]
    public ActionResult<NatalChartResult> Calculate([FromBody] BirthData birthData)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // If TimeZoneId not provided, resolve from coordinates and convert to UTC
        if (string.IsNullOrEmpty(birthData.TimeZoneId))
        {
            birthData.TimeZoneId = _timeZoneService.GetTimeZoneId(birthData.Latitude, birthData.Longitude);
        }

        // Parse the local birth time and convert to UTC
        var timeParts = birthData.BirthTime.Split(':');
        var hour = int.Parse(timeParts[0]);
        var minute = timeParts.Length > 1 ? int.Parse(timeParts[1]) : 0;

        var localDateTime = new DateTime(
            birthData.BirthDate.Year,
            birthData.BirthDate.Month,
            birthData.BirthDate.Day,
            hour, minute, 0
        );

        var utcDateTime = _timeZoneService.ConvertToUtc(localDateTime, birthData.Latitude, birthData.Longitude);

        // Update birthData to UTC for the calculator
        birthData.BirthDate = utcDateTime.Date;
        birthData.BirthTime = utcDateTime.ToString("HH:mm");

        var result = _chartCalculator.Calculate(birthData);
        return Ok(result);
    }

    [HttpPost("interpret")]
    public ActionResult<InterpretationResult> Interpret([FromBody] NatalChartResult chart, [FromQuery] string lang = "en")
    {
        var result = _interpretationService.GetInterpretations(chart, lang);
        return Ok(result);
    }
}
