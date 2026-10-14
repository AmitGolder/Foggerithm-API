
namespace Foggerithm.Api.Models;

public class DeviceActivity
{
    public int Id { get; set; }

    public string DeviceId { get; set; } = "fogger-001";

    public string EventType { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
