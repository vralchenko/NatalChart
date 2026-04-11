using Microsoft.AspNetCore.Mvc;
using NatalChart.Api.Helpers;
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
    private readonly INumerologyService _numerologyService;

    public ChartController(
        IChartCalculator chartCalculator,
        IInterpretationService interpretationService,
        ITimeZoneService timeZoneService,
        INumerologyService numerologyService)
    {
        _chartCalculator = chartCalculator;
        _interpretationService = interpretationService;
        _timeZoneService = timeZoneService;
        _numerologyService = numerologyService;
    }

    [HttpPost("calculate")]
    public ActionResult<NatalChartResult> Calculate([FromBody] BirthData birthData)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        BirthDataHelper.ResolveTimezoneAndConvertToUtc(birthData, _timeZoneService);

        var result = _chartCalculator.Calculate(birthData);
        return Ok(result);
    }

    [HttpPost("numerology")]
    public ActionResult<NumerologyResult> Numerology([FromBody] NumerologyRequest request, [FromQuery] string lang = "en")
    {
        var result = _numerologyService.Calculate(request.BirthDate, lang);
        return Ok(result);
    }

    [HttpPost("interpret")]
    public ActionResult<InterpretationResult> Interpret([FromBody] NatalChartResult chart, [FromQuery] string lang = "en")
    {
        var result = _interpretationService.GetInterpretations(chart, lang);
        return Ok(result);
    }
}
