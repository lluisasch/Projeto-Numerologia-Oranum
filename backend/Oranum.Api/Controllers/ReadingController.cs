using Microsoft.AspNetCore.Mvc;
using Oranum.Application.Abstractions;
using Oranum.Application.DTOs.Requests;

namespace Oranum.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ReadingController : ControllerBase
{
    private readonly IReadingService _readingService;

    public ReadingController(IReadingService readingService)
    {
        _readingService = readingService;
    }

    [HttpPost("name")]
    public async Task<IActionResult> GenerateNameReading([FromBody] NameReadingRequest request, CancellationToken cancellationToken)
    {
        var response = await _readingService.GenerateNameReadingAsync(request, cancellationToken);
        return Ok(response);
    }

    [HttpPost("birthdate")]
    public async Task<IActionResult> GenerateBirthDateReading([FromBody] BirthDateReadingRequest request, CancellationToken cancellationToken)
    {
        var response = await _readingService.GenerateBirthDateReadingAsync(request, cancellationToken);
        return Ok(response);
    }

    [HttpPost("compatibility")]
    public async Task<IActionResult> GenerateCompatibilityReading([FromBody] CompatibilityReadingRequest request, CancellationToken cancellationToken)
    {
        var response = await _readingService.GenerateCompatibilityReadingAsync(request, cancellationToken);
        return Ok(response);
    }
}
