using TenantApi.Dto;

namespace TenantApi.Services;

public interface IBookingService
{
    Task<List<BookingDto>> GetBookingsForUserBuilding(Guid userId, Guid? machineId = null);
    Task<BookingDto> CreateBooking(Guid userId, CreateBookingRequest request);
    Task DeleteBooking(Guid userId, Guid bookingId);
    Task<List<MachineDto>> GetMachinesForUserBuilding(Guid userId);
}
