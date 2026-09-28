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

        /// <summary>
        /// List bookings for the caller's building. Optional machineId filter.
        /// </summary>
        [HttpGet("getAll")]
        public async Task<ActionResult<List<BookingDto>>> GetAllBookings([FromQuery] Guid? machineId = null)
        {
            if (!Guid.TryParse(User.FindFirstValue(TenantClaims.UserId), out var userId))
            {
                throw new CustomException("Invalid user id", null, 401);
            }

            var bookings = await _bookingService.GetBookingsForUserBuilding(userId, machineId);
            return Ok(bookings);
        }

        [HttpPost("create")]
        public IActionResult CreateBooking()
            => StatusCode(StatusCodes.Status501NotImplemented, new { message = NotMigrated });

        [HttpPost("createnew")]
        public IActionResult CreateBookingNew()
            => StatusCode(StatusCodes.Status501NotImplemented, new { message = NotMigrated });

        [HttpPost("edit")]
        public IActionResult EditBookingById()
            => StatusCode(StatusCodes.Status501NotImplemented, new { message = NotMigrated });

        [HttpPost("cancel")]
        public IActionResult CancelBookings()
            => StatusCode(StatusCodes.Status501NotImplemented, new { message = NotMigrated });

        [HttpGet("getAllMachines")]
        public IActionResult GetAllMachinesByBuildingId()
            => StatusCode(StatusCodes.Status501NotImplemented, new { message = NotMigrated });
    }
}
