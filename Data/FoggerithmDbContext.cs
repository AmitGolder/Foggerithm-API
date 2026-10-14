
using Foggerithm.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Foggerithm.Api.Data;

public class FoggerithmDbContext
    : IdentityDbContext<ApplicationUser>
{
    public FoggerithmDbContext(
        DbContextOptions<FoggerithmDbContext> options)
        : base(options)
    {
    }

    public DbSet<FoggerSchedule> Schedules => Set<FoggerSchedule>();

    public DbSet<DeviceStatus> Devices => Set<DeviceStatus>();

    public DbSet<DeviceActivity> DeviceActivities => Set<DeviceActivity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DeviceStatus>()
            .HasIndex(device => device.DeviceId)
            .IsUnique();
    }
}
