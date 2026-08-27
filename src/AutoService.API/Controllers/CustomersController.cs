using AutoService.API.Contracts.Customers;
using AutoService.Application.Customers;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.API.Controllers;

[ApiController]
[Route("api/customers")]
public sealed class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CustomerResponse>> Create(
        CreateCustomerRequest request,
        CancellationToken cancellationToken)
    {
        var customer = await _customerService.CreateAsync(
            request.FirstName,
            request.LastName,
            request.PhoneNumber,
            request.Email,
            cancellationToken);

        var response = new CustomerResponse
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            PhoneNumber = customer.PhoneNumber,
            Email = customer.Email,
            CreatedAtUtc = customer.CreatedAtUtc
        };

        return Created($"/api/customers/{customer.Id}", response);
    }
}
