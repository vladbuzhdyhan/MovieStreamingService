using Microsoft.AspNetCore.Mvc;
using MovieStreamingService.Application.Services;

namespace MovieStreamingService.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DataSeederController : ControllerBase
{
    private readonly DataSeederService dataSeederService;

    public DataSeederController(DataSeederService dataSeederService)
    {
        this.dataSeederService = dataSeederService;
    }

    [HttpPost("db-seed")]
    public async Task<IActionResult> SeedData()
    {
        await dataSeederService.SeedDataAsync();
        return Ok();
    }
}