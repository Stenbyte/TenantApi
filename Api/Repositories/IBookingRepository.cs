using TenantApi.Dto;

namespace TenantApi.Repository;

public interface IBookingRepository
{
    Task<List<BookingDto>> GetBookingsByBuildingId(Guid buildingId, Guid? machineId = null);
}
