using Microsoft.AspNetCore.Mvc;
using NatalChart.Api.Helpers;
using NatalChart.Api.Services;
using NatalChart.Core.Interfaces;
using NatalChart.Core.Models;
using NatalChart.Infrastructure;

namespace NatalChart.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExportController : ControllerBase
{
    private readonly IChartCalculator _chartCalculator;
    private readonly IInterpretationService _interpretationService;
    private readonly INumerologyService _numerologyService;
    private readonly ITimeZoneService _timeZoneService;
    private readonly PdfExportService _pdfExportService;

    public ExportController(
        IChartCalculator chartCalculator,
        IInterpretationService interpretationService,
        INumerologyService numerologyService,
        ITimeZoneService timeZoneService,
        PdfExportService pdfExportService)
    {
        _chartCalculator = chartCalculator;
        _interpretationService = interpretationService;
        _numerologyService = numerologyService;
        _timeZoneService = timeZoneService;
        _pdfExportService = pdfExportService;
    }

    public class PdfExportRequest
    {
        public BirthData BirthData { get; set; } = null!;
        public string Lang { get; set; } = "en";
        public string LocationName { get; set; } = "";
    }

    [HttpPost("pdf")]
    public ActionResult ExportPdf([FromBody] PdfExportRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Keep original date for numerology (before UTC conversion)
        var originalDate = request.BirthData.BirthDate;

        BirthDataHelper.ResolveTimezoneAndConvertToUtc(request.BirthData, _timeZoneService);

        var chart = _chartCalculator.Calculate(request.BirthData);
        var interpretations = _interpretationService.GetInterpretations(chart, request.Lang);
        var numerology = _numerologyService.Calculate(originalDate, request.Lang);

        var pdfBytes = _pdfExportService.GeneratePdf(
            request.BirthData,
            request.LocationName,
            chart,
            interpretations,
            numerology,
            request.Lang);

        var fileName = $"natal-chart-{originalDate:yyyy-MM-dd}.pdf";
        return File(pdfBytes, "application/pdf", fileName);
    }
}
