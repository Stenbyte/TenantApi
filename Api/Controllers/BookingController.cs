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
        private readonly IBookingService _bookingService = bookingService;

        [HttpGet("getAll")]
        public async Task<ActionResult<List<BookingDto>>> GetAllBookings([FromQuery] Guid? machineId = null)
        {
            Guid userId = RequireUserId();
            List<BookingDto> bookings = await _bookingService.GetBookingsForUserBuilding(userId, machineId);
            return Ok(bookings);
        }

        [HttpGet("getAllMachines")]
        public async Task<ActionResult<List<MachineDto>>> GetAllMachines()
        {
            Guid userId = RequireUserId();
            List<MachineDto> machines = await _bookingService.GetMachinesForUserBuilding(userId);
            return Ok(machines);
        }

        [HttpPost("create")]
        public async Task<ActionResult<BookingDto>> CreateBooking([FromBody] CreateBookingRequest request)
        {
            Guid userId = RequireUserId();
            BookingDto booking = await _bookingService.CreateBooking(userId, request);
            return Ok(booking);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> EditBookingById([FromBody] EditBookingRequest request)
        {
            Guid userId = RequireUserId();
            await _bookingService.DeleteBooking(userId, request.Id);
            return Ok(new { message = "Booking removed" });
        }

        [HttpPost("cancel")]
        public async Task<IActionResult> CancelBookings()
        {
            Guid userId = RequireUserId();
            int removed = await _bookingService.CancelAllBookings(userId);
            return Ok(new { message = "Bookings canceled", removed });
        }

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
