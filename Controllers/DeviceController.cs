
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
public class DeviceController : ControllerBase
{
    private readonly FoggerithmDbContext _context;

    public DeviceController(FoggerithmDbContext context)
    {
        _context = context;
    }

    private string? GetCurrentUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    [HttpGet("status")]
    public async Task<ActionResult<DeviceStatus>> GetStatus()
    {
        var userId = GetCurrentUserId();

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var device = await _context.Devices
            .FirstOrDefaultAsync(d => d.OwnerUserId == userId);

        // Do not expose another customer's device or create one
        // automatically during a status request.
        if (device is null)
            return NotFound(new
            {
                message = "No device is assigned to your account."
            });

        return Ok(device);
    }

    [HttpPost("power")]
    public async Task<ActionResult<DeviceStatus>> SetPower(
        [FromBody] PowerRequest request)
    {
        var userId = GetCurrentUserId();

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var device = await _context.Devices
            .FirstOrDefaultAsync(d => d.OwnerUserId == userId);

        if (device is null)
            return NotFound(new
            {
                message = "No device is assigned to your account."
            });

        // Simulation only: this does not control physical hardware.
        var wasOn = device.IsFoggerOn;
        var now = DateTime.UtcNow;

        device.IsFoggerOn = request.IsOn;
        device.LastUpdated = now;

        _context.DeviceActivities.Add(new DeviceActivity
        {
            DeviceId = device.DeviceId,
            EventType = request.IsOn ? "PowerOn" : "PowerOff",
            Message = wasOn == request.IsOn
                ? $"Power command received; fogger was already {(request.IsOn ? "on" : "off")}."
                : $"Fogger turned {(request.IsOn ? "on" : "off")} in simulation.",
            Timestamp = now
        });

        await _context.SaveChangesAsync();

        return Ok(device);
    }
}

public class PowerRequest
{
    public bool IsOn { get; set; }
}
