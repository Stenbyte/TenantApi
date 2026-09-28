using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TenantApi.Models;

/// <summary>
/// Per-building booking rules. 1:1 with Building. Defaults applied on first use.
/// </summary>
[Table("building_settings")]
public class BuildingSettings
{
    [Key]
    [Column("building_id")]
    public Guid BuildingId { get; set; }

    public Building Building { get; set; } = null!;

    /// <summary>Max bookings one tenant may hold at once (default 3).</summary>
    [Column("max_bookings_per_week")]
    public int MaxBookingsPerWeek { get; set; } = 3;

    /// <summary>Slot duration foundation (minutes). Current fixed slots are 180.</summary>
    [Column("slot_length_minutes")]
    public int SlotLengthMinutes { get; set; } = 180;
}
