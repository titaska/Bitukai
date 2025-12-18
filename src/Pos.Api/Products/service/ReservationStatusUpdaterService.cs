using Microsoft.EntityFrameworkCore;
using Pos.Api.Context;
using Pos.Api.reservations.model;

namespace Pos.Api.reservations.services
{
    public class ReservationStatusUpdaterService : BackgroundService
    {
        private readonly IServiceProvider _services;

        public ReservationStatusUpdaterService(IServiceProvider services)
        {
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var now = DateTime.UtcNow;

            var toUpdate = await db.Set<Reservation>()
                .Where(r => r.Status == "BOOKED" && r.StartTime <= now)
                .ToListAsync(stoppingToken);

            if (toUpdate.Count > 0)
            {
                foreach (var r in toUpdate) r.Status = "COMPLETED";
                await db.SaveChangesAsync(stoppingToken);
            }
        }
    }
}
