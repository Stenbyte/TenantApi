using TenantApi.Dto;

namespace TenantApi.Repository;

public interface IBookingRepository
{
    Task<List<BookingDto>> GetBookingsByBuildingId(Guid buildingId, Guid? machineId = null);
    Task<BookingDto> CreateBooking(Guid userId, Guid buildingId, CreateBookingRequest request);
    Task DeleteBookingForUser(Guid userId, Guid bookingId);
    Task<int> CancelAllBookingsForUser(Guid userId);
    Task<List<MachineDto>> GetMachinesByBuildingId(Guid buildingId);
    Task EnsureDefaultMachines(Guid buildingId);
}
