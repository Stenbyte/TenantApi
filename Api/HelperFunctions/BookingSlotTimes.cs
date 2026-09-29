using TenantApi.Exceptions;

namespace TenantApi.Helpers;

/// <summary>
/// Maps FE-style day + "HH:mm-HH:mm" slots to UTC timestamps (Denmark local).
/// </summary>
public static class BookingSlotTimes
{
    public static readonly IReadOnlyList<string> AllowedSlots =
    [
        "08:00-11:00",
        "11:00-14:00",
        "14:00-17:00",
        "17:00-20:00"
    ];

    private static readonly TimeZoneInfo DenmarkTz = ResolveDenmarkTimeZone();

    public static (DateTime StartUtc, DateTime EndUtc) ToUtcRange(string day, string timeSlot)
    {
        var date = ParseDay(day);

        if (string.IsNullOrWhiteSpace(timeSlot) || !AllowedSlots.Contains(timeSlot))
        {
            throw new CustomException(
                $"Invalid timeSlot. Allowed: {string.Join(", ", AllowedSlots)}",
                null,
                400);
        }

        var parts = timeSlot.Split('-', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2
            || !TimeOnly.TryParse(parts[0], out var startLocal)
            || !TimeOnly.TryParse(parts[1], out var endLocal))
        {
            throw new CustomException("timeSlot must look like \"08:00-11:00\"", null, 400);
        }

        if (endLocal <= startLocal)
        {
            throw new CustomException("Slot end must be after slot start", null, 400);
        }

        var startUnspec = date.ToDateTime(startLocal, DateTimeKind.Unspecified);
        var endUnspec = date.ToDateTime(endLocal, DateTimeKind.Unspecified);

        var startUtc = TimeZoneInfo.ConvertTimeToUtc(startUnspec, DenmarkTz);
        var endUtc = TimeZoneInfo.ConvertTimeToUtc(endUnspec, DenmarkTz);

        return (startUtc, endUtc);
    }

    public static DateOnly ParseDay(string day)
    {
        if (string.IsNullOrWhiteSpace(day))
        {
            throw new CustomException("day is required (e.g. \"2026-09-29\")", null, 400);
        }

        if (DateOnly.TryParse(day, out var dateOnly))
        {
            return dateOnly;
        }

        if (DateTime.TryParse(day, null, System.Globalization.DateTimeStyles.RoundtripKind, out var dateTime))
        {
            return DateOnly.FromDateTime(dateTime);
        }

        throw new CustomException("day must be a date like \"2026-09-29\"", null, 400);
    }

    private static TimeZoneInfo ResolveDenmarkTimeZone()
    {
        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById("Europe/Copenhagen");
        }
        catch (TimeZoneNotFoundException)
        {
            // Windows
            return TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
        }
    }
}
