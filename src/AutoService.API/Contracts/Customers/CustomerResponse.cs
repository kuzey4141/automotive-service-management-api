namespace AutoService.API.Contracts.Customers;

public sealed class CustomerResponse
{
    public Guid Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public string? Email { get; init; }
    public DateTime CreatedAtUtc { get; init; }
}
