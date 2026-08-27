using System.ComponentModel.DataAnnotations;

namespace AutoService.API.Contracts.Customers;

public sealed class UpdateCustomerRequest
{
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;

    [EmailAddress]
    [MaxLength(200)]
    public string? Email { get; set; }
}
