
using Foggerithm.Api.Data;
using Foggerithm.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Foggerithm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeviceActivitiesController : ControllerBase
{
    private readonly FoggerithmDbContext _context;

    public DeviceActivitiesController(FoggerithmDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<DeviceActivity>>> GetActivities(
        [FromQuery] string deviceId = "fogger-001",
        [FromQuery] int limit = 50)
    {
        limit = Math.Clamp(limit, 1, 200);

        var activities = await _context.DeviceActivities
            .Where(a => a.DeviceId == deviceId)
            .OrderByDescending(a => a.Timestamp)
            .Take(limit)
            .ToListAsync();

        return Ok(activities);
    }
}
