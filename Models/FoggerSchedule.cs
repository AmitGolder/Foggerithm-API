
namespace Foggerithm.Api.Models;

public class FoggerSchedule
{
    public int Id { get; set; }

    // Identity user ID of the schedule owner.
    // Nullable temporarily for existing database records.
    public string? OwnerUserId { get; set; }

    public string Time { get; set; } = "07:00";

    public int Duration { get; set; } = 10;

    public bool Enabled { get; set; } = true;
}
