using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TenantApi.Exceptions;

namespace TenantApi.Dto;

public class CreateBookingRequest
{
    /// <summary>
    /// Optional until landlord machine admin exists. If omitted, first available machine in the building is used.
    /// </summary>
    public Guid? MachineId { get; init; }

    /// <summary>Calendar day, e.g. "2026-09-29".</summary>
    [Required]
    public string Day { get; init; } = null!;

    /// <summary>Single slot, e.g. "08:00-11:00".</summary>
    public string? TimeSlot { get; init; }

    /// <summary>FE shape: ["08:00-11:00"]. First entry is used for V1.</summary>
    public List<string>? TimeSlots { get; init; }

    [JsonIgnore]
    public string ResolvedTimeSlot
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(TimeSlot))
            {
                return TimeSlot;
            }

            if (TimeSlots is { Count: > 0 } && !string.IsNullOrWhiteSpace(TimeSlots[0]))
            {
                return TimeSlots[0];
            }

            throw new CustomException(
                "timeSlot or timeSlots is required (e.g. \"08:00-11:00\")",
                null,
                400);
        }
    }
}

public class MachineDto
{
    public Guid Id { get; init; }
    public Guid BuildingId { get; init; }
    public string Name { get; init; } = null!;
    public string Status { get; init; } = null!;
}
