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
            Guid buildingId = await RequireBuildingId(userId);
            List<BookingDto> bookings = await _bookingRepository.GetBookingsByBuildingId(buildingId, machineId);
            return bookings;
        }

        public async Task<BookingDto> CreateBooking(Guid userId, CreateBookingRequest request)
        {
            Guid buildingId = await RequireBuildingId(userId);
            BookingDto booking = await _bookingRepository.CreateBooking(userId, buildingId, request);
            return booking;
        }

        public async Task DeleteBooking(Guid userId, Guid bookingId)
        {
            await _bookingRepository.DeleteBookingForUser(userId, bookingId);
        }

        public async Task<int> CancelAllBookings(Guid userId)
        {
            int removed = await _bookingRepository.CancelAllBookingsForUser(userId);
            return removed;
        }

        public async Task<List<MachineDto>> GetMachinesForUserBuilding(Guid userId)
        {
            Guid buildingId = await RequireBuildingId(userId);
            List<MachineDto> machines = await _bookingRepository.GetMachinesByBuildingId(buildingId);
            return machines;
        }

        private async Task<Guid> RequireBuildingId(Guid userId)
        {
            var buildingId = await _userRepository.GetBuildingIdForUser(userId);
            if (buildingId is null)
            {
                throw new CustomException("User is not linked to a building", null, 404);
            }

            if (buildingId.Value == Guid.Empty)
            {
                throw new CustomException("Building ID is empty", null, 404);
            }

            return buildingId.Value;
        }
    }
}
