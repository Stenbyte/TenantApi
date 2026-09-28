using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TenantApi.Controllers;
using TenantApi.Dto;
using TenantApi.Exceptions;
using TenantApi.Services;
using TenantApi.Shared.Constansts;

namespace TenantApi.Laundry.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class BookingController(IBookingService bookingService) : AppControllerBase
    {
        private const string NotMigrated = "Bookings not migrated to Postgres yet";
        private readonly IBookingService _bookingService = bookingService;

        [HttpGet("getAll")]
        public async Task<ActionResult<List<BookingDto>>> GetAllBookings([FromQuery] Guid? machineId = null)
        {
            var userId = RequireUserId();
            var bookings = await _bookingService.GetBookingsForUserBuilding(userId, machineId);
            return Ok(bookings);
        }

        [HttpGet("getAllMachines")]
        public async Task<ActionResult<List<MachineDto>>> GetAllMachines()
        {
            var userId = RequireUserId();
            var machines = await _bookingService.GetMachinesForUserBuilding(userId);
            return Ok(machines);
        }

        [HttpPost("create")]
        public async Task<ActionResult<BookingDto>> CreateBooking([FromBody] CreateBookingRequest request)
        {
            var userId = RequireUserId();
            var booking = await _bookingService.CreateBooking(userId, request);
            return CreatedAtAction(nameof(GetAllBookings), new { machineId = booking.MachineId }, booking);
        }

        [HttpPost("createnew")]
        public IActionResult CreateBookingNew()
            => StatusCode(StatusCodes.Status501NotImplemented, new { message = NotMigrated });

        [HttpPost("edit")]
        public IActionResult EditBookingById()
            => StatusCode(StatusCodes.Status501NotImplemented, new { message = NotMigrated });

        [HttpPost("cancel")]
        public IActionResult CancelBookings()
            => StatusCode(StatusCodes.Status501NotImplemented, new { message = NotMigrated });

        private Guid RequireUserId()
        {
            if (!Guid.TryParse(User.FindFirstValue(TenantClaims.UserId), out var userId))
            {
                throw new CustomException("Invalid user id", null, 401);
            }

            return userId;
        }
    }
}
