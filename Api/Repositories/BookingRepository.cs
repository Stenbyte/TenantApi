using TenantApi.Exceptions;
using TenantApi.Models;

namespace TenantApi.Repository;

class BookingRepository : IBookingRepository
{
    private static CustomException NotMigrated()
        => new("Bookings not migrated to Postgres yet", null, 501);

    public Task<List<Booking>> GetAllBookingsByBuildingId(User user)
        => throw NotMigrated();

    public Task<List<Booking>> GetAllBookingsByMachineId(User user, string machineId)
        => throw NotMigrated();

    public Task<Booking> CreateBooking(Booking newBooking, string dbName)
        => throw NotMigrated();

    public Task<Booking> UpdateBooking(Booking existingBooking, string dbName)
        => throw NotMigrated();

    public Task<Booking> GetBookingsByUserId(string userId, string dbName)
        => throw NotMigrated();

    public Task<Booking> FindByUserAndSlotId(string bookingSlotId, string userId, string dbName)
        => throw NotMigrated();

    public Task<Booking> FindBookingsByUserId(string userId, string dbName)
        => throw NotMigrated();

    public Task<bool> CancelBooking(string userId, string dbName)
        => throw NotMigrated();

    public Task<MachineModel> GetMachine(string dbName, string machineId)
        => throw NotMigrated();

    public Task<List<MachineModel>> GetAllMachinesByBuildingId(User user)
        => throw NotMigrated();

    public Task<MachineModel> CreateMachine(string dbName, MachineModel newMachine)
        => throw NotMigrated();
}
