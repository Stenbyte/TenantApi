using TenantApi.Dto;
using TenantApi.Models;

namespace TenantApi.Repository;

public interface IBookingRepository
{
    Task<List<BookingDto>> GetBookingsByBuildingId(Guid buildingId, Guid? machineId = null);
    Task<BookingDto> CreateBooking(Guid userId, Guid buildingId, CreateBookingRequest request);
    Task<List<MachineDto>> GetMachinesByBuildingId(Guid buildingId);
    Task EnsureDefaultMachines(Guid buildingId);
}
