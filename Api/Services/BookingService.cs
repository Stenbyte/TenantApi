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
            var buildingId = await _userRepository.GetBuildingIdForUser(userId);
            if (buildingId is null)
            {
                throw new CustomException("User is not linked to a building", null, 404);
            }

            return await _bookingRepository.GetBookingsByBuildingId(buildingId.Value, machineId);
        }
    }
}
