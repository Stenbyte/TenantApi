using Microsoft.EntityFrameworkCore;
using TenantApi.Dto;
using TenantApi.Models;

namespace TenantApi.Repository;

public class BookingRepository : IBookingRepository
{
    private readonly TenantDbContext _dbContext;

    public BookingRepository(TenantDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<BookingDto>> GetBookingsByBuildingId(Guid buildingId, Guid? machineId = null)
    {
        var query = _dbContext.Bookings.AsNoTracking()
            .Where(b => b.BuildingId == buildingId);

        if (machineId.HasValue)
        {
            query = query.Where(b => b.MachineId == machineId.Value);
        }

        return await query
            .OrderBy(b => b.StartTime)
            .Select(b => new BookingDto {
                Id = b.Id,
                UserId = b.UserId,
                BuildingId = b.BuildingId,
                MachineId = b.MachineId,
                StartTime = b.StartTime,
                EndTime = b.EndTime,
                CreatedAt = b.CreatedAt
            })
            .ToListAsync();
    }
}
