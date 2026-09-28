using TenantApi.Dto;
using TenantApi.Exceptions;
using TenantApi.Repository;
using TenantApi.Services;

namespace LaundryBooking.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;

        public BookingService(IBookingRepository bookingRepository, IUserRepository userRepository)
        {
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
        }

        public async Task<List<BookingDto>> GetBookingsForUserBuilding(Guid userId, Guid? machineId = null)
        {
            var buildingId = await RequireBuildingId(userId);
            return await _bookingRepository.GetBookingsByBuildingId(buildingId, machineId);
        }

        public async Task<BookingDto> CreateBooking(Guid userId, CreateBookingRequest request)
        {
            var buildingId = await RequireBuildingId(userId);
            return await _bookingRepository.CreateBooking(userId, buildingId, request);
        }

        public async Task<List<MachineDto>> GetMachinesForUserBuilding(Guid userId)
        {
            var buildingId = await RequireBuildingId(userId);
            return await _bookingRepository.GetMachinesByBuildingId(buildingId);
        }

        private async Task<Guid> RequireBuildingId(Guid userId)
        {
            var buildingId = await _userRepository.GetBuildingIdForUser(userId);
            if (buildingId is null)
            {
                throw new CustomException("User is not linked to a building", null, 404);
            }

            return buildingId.Value;
        }
    }
}
