namespace TenantApi.Dto;

/// <summary>
/// API response for one reserved laundry slot (Postgres booking row).
/// </summary>
public class BookingDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public Guid BuildingId { get; init; }
    public Guid MachineId { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public DateTime CreatedAt { get; init; }
}
