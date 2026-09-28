using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TenantApi.Enums;

namespace TenantApi.Models
{
    [Table("machines")]
    public class Machine
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column("building_id")]
        public Guid BuildingId { get; set; }

        public Building Building { get; set; } = null!;

        [Required]
        [Column("name")]
        public MachineName Name { get; set; }

        [Required]
        [Column("status")]
        public MachineStatus Status { get; set; } = MachineStatus.available;

        public ICollection<BookingPg> Bookings { get; set; } = new List<BookingPg>();
    }

    /// <summary>
    /// One row = one reserved slot. Unique (machine_id, start_time) prevents double-booking.
    /// </summary>
    [Table("bookings")]
    public class BookingPg
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column("user_id")]
        public Guid UserId { get; set; }

        public UserPg User { get; set; } = null!;

        /// <summary>Tenant/building isolation key (shared-DB V1).</summary>
        [Required]
        [Column("building_id")]
        public Guid BuildingId { get; set; }

        public Building Building { get; set; } = null!;

        [Required]
        [Column("machine_id")]
        public Guid MachineId { get; set; }

        public Machine Machine { get; set; } = null!;

        [Required]
        [Column("start_time")]
        public DateTime StartTime { get; set; }

        [Required]
        [Column("end_time")]
        public DateTime EndTime { get; set; }

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
