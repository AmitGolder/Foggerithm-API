
namespace Foggerithm.Api.Models;

public class DeviceStatus
{
    public int Id { get; set; }

    public string DeviceId { get; set; } = "fogger-001";

    public string DeviceName { get; set; } = "Greenhouse Fogger 01";

    // Identity user ID of the device owner.
    // Nullable temporarily for existing database records.
    public string? OwnerUserId { get; set; }

    public bool IsConnected { get; set; } = false;

    public bool IsFoggerOn { get; set; } = false;

    public string ConnectionType { get; set; } = "Not connected";

    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}
