using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TenantApi.Controllers;

namespace TenantApi.Laundry.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class BookingController : AppControllerBase
    {
        private const string NotMigrated = "Bookings not migrated to Postgres yet";

        [HttpGet("getAll")]
        public IActionResult GetAllBookingsByMachineId()
            => StatusCode(StatusCodes.Status501NotImplemented, new { message = NotMigrated });

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
