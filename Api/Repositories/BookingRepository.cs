using Microsoft.EntityFrameworkCore;
using Npgsql;
using TenantApi.Dto;
using TenantApi.Enums;
using TenantApi.Exceptions;
using TenantApi.Helpers;
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

    public async Task EnsureDefaultMachines(Guid buildingId)
    {
        var hasAny = await _dbContext.Machines.AnyAsync(m => m.BuildingId == buildingId);
        if (hasAny)
        {
            return;
        }

        _dbContext.Machines.AddRange(
            new Machine {
                BuildingId = buildingId,
                Name = MachineName.washing,
                Status = MachineStatus.available
            },
            new Machine {
                BuildingId = buildingId,
                Name = MachineName.dryer,
                Status = MachineStatus.available
            });

        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<MachineDto>> GetMachinesByBuildingId(Guid buildingId)
    {
        await EnsureDefaultMachines(buildingId);

        return await _dbContext.Machines.AsNoTracking()
            .Where(m => m.BuildingId == buildingId)
            .OrderBy(m => m.Name)
            .Select(m => new MachineDto {
                Id = m.Id,
                BuildingId = m.BuildingId,
                Name = m.Name.ToString(),
                Status = m.Status.ToString()
            })
            .ToListAsync();
    }

    public async Task<BookingDto> CreateBooking(Guid userId, Guid buildingId, CreateBookingRequest request)
    {
        var (start, end) = BookingSlotTimes.ToUtcRange(request.Day, request.ResolvedTimeSlot);

        if (end <= DateTime.UtcNow)
        {
            throw new CustomException("Cannot book a slot that has already ended", null, 400);
        }

        await EnsureDefaultMachines(buildingId);

        Machine? machine;
        if (request.MachineId.HasValue)
        {
            machine = await _dbContext.Machines
                .FirstOrDefaultAsync(m => m.Id == request.MachineId.Value && m.BuildingId == buildingId);

            if (machine is null)
            {
                throw new CustomException("Machine not found for this building", null, 404);
            }
        }
        else
        {
            // Temporary: landlord machine admin not built yet — pick first available.
            machine = await _dbContext.Machines
                .Where(m => m.BuildingId == buildingId && m.Status == MachineStatus.available)
                .OrderBy(m => m.Name)
                .FirstOrDefaultAsync();

            if (machine is null)
            {
                throw new CustomException("No available machine in this building", null, 404);
            }
        }

        if (machine.Status == MachineStatus.maintenance)
        {
            throw new CustomException("Machine is under maintenance", null, 403);
        }

        var booking = new BookingPg {
            UserId = userId,
            BuildingId = buildingId,
            MachineId = machine.Id,
            StartTime = start,
            EndTime = end,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Bookings.Add(booking);

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (IsUniqueViolation(ex))
        {
            // DB unique (machine_id, start_time) — race-safe double-book guard
            throw new CustomException("Time slot is already taken", null, 409);
        }

        return ToDto(booking);
    }

    public async Task DeleteBookingForUser(Guid userId, Guid bookingId)
    {
        if (bookingId == Guid.Empty)
        {
            throw new CustomException("Booking id is required", null, 400);
        }

        var booking = await _dbContext.Bookings
            .FirstOrDefaultAsync(b => b.Id == bookingId);

        if (booking is null)
        {
            throw new CustomException("Booking not found", null, 404);
        }

        if (booking.UserId != userId)
        {
            throw new CustomException("You can only remove your own booking", null, 403);
        }

        _dbContext.Bookings.Remove(booking);
        await _dbContext.SaveChangesAsync();
    }

    private static bool IsUniqueViolation(DbUpdateException ex)
        => ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation };

    private static BookingDto ToDto(BookingPg b) => new() {
        Id = b.Id,
        UserId = b.UserId,
        BuildingId = b.BuildingId,
        MachineId = b.MachineId,
        StartTime = b.StartTime,
        EndTime = b.EndTime,
        CreatedAt = b.CreatedAt
    };
}
