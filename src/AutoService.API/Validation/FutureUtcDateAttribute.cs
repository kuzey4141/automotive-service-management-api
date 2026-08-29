using System.ComponentModel.DataAnnotations;

namespace AutoService.API.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public sealed class FutureUtcDateAttribute : ValidationAttribute
{
    public FutureUtcDateAttribute()
        : base("The date must be in the future.")
    {
    }

    public override bool IsValid(object? value)
    {
        if (value is not DateTime dateTime || dateTime == default)
        {
            return true;
        }

        return NormalizeUtc(dateTime) > DateTime.UtcNow;
    }

    private static DateTime NormalizeUtc(DateTime dateTime)
    {
        return dateTime.Kind switch
        {
            DateTimeKind.Utc => dateTime,
            DateTimeKind.Local => dateTime.ToUniversalTime(),
            _ => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc)
        };
    }
}
