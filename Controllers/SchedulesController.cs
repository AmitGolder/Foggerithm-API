
using System.Security.Claims;
using Foggerithm.Api.Data;
using Foggerithm.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Foggerithm.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SchedulesController : ControllerBase
{
    private readonly FoggerithmDbContext _context;

    public SchedulesController(FoggerithmDbContext context)
    {
        _context = context;
    }

    private string? GetCurrentUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    [HttpGet]
    public async Task<ActionResult<List<FoggerSchedule>>> GetSchedules()
    {
        var userId = GetCurrentUserId();

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var schedules = await _context.Schedules
            .Where(s => s.OwnerUserId == userId)
            .OrderBy(s => s.Time)
            .ToListAsync();

        return Ok(schedules);
    }

    [HttpPost]
    public async Task<ActionResult<FoggerSchedule>> AddSchedule(
        [FromBody] FoggerSchedule schedule)
    {
        var userId = GetCurrentUserId();

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        if (!TimeOnly.TryParse(schedule.Time, out _))
            return BadRequest("Invalid time. Use HH:mm format.");

        if (schedule.Duration < 1 || schedule.Duration > 1440)
            return BadRequest(
                "Duration must be between 1 and 1440 minutes.");

        schedule.Id = 0;

        // Never trust the owner ID supplied by the client.
        schedule.OwnerUserId = userId;

        _context.Schedules.Add(schedule);
        await _context.SaveChangesAsync();

        return Ok(schedule);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteSchedule(int id)
    {
        var userId = GetCurrentUserId();

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var schedule = await _context.Schedules
            .FirstOrDefaultAsync(s =>
                s.Id == id && s.OwnerUserId == userId);

        if (schedule is null)
            return NotFound();

        _context.Schedules.Remove(schedule);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateSchedule(
        int id,
        [FromBody] FoggerSchedule updated)
    {
        var userId = GetCurrentUserId();

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var schedule = await _context.Schedules
            .FirstOrDefaultAsync(s =>
                s.Id == id && s.OwnerUserId == userId);

        if (schedule is null)
            return NotFound();

        if (!TimeOnly.TryParse(updated.Time, out _))
            return BadRequest("Invalid time. Use HH:mm format.");

        if (updated.Duration < 1 || updated.Duration > 1440)
            return BadRequest(
                "Duration must be between 1 and 1440 minutes.");

        schedule.Time = updated.Time;
        schedule.Duration = updated.Duration;
        schedule.Enabled = updated.Enabled;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
