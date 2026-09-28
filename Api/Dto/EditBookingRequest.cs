using System.ComponentModel.DataAnnotations;

namespace TenantApi.Dto;

/// <summary>
/// Removes one booking row (FE "edit" = cancel that slot).
/// </summary>
public class EditBookingRequest
{
    [Required]
    public Guid Id { get; init; }
}
