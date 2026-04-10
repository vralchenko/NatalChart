using Microsoft.AspNetCore.Mvc;
using NatalChart.Core.Interfaces;
using NatalChart.Core.Models;

namespace NatalChart.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GeocodingController : ControllerBase
{
    private readonly IGeocodingService _geocodingService;

    public GeocodingController(IGeocodingService geocodingService)
    {
        _geocodingService = geocodingService;
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<GeocodingResult>>> Search([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
            return BadRequest("Query must be at least 2 characters");

        var results = await _geocodingService.SearchAsync(query);
        return Ok(results);
    }
}
