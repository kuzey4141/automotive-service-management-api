using AutoService.API.Contracts.Customers;
using AutoService.Application.Customers;
using AutoService.Domain.Entities;
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

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CustomerResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<CustomerResponse>>> GetAll(
        CancellationToken cancellationToken)
    {
        var customers = await _customerService.GetAllAsync(cancellationToken);
        var response = customers.Select(ToResponse).ToList();

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var customer = await _customerService.GetByIdAsync(id, cancellationToken);

        if (customer is null)
        {
            return NotFound();
        }

        return Ok(ToResponse(customer));
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

        var response = ToResponse(customer);

        return CreatedAtAction(nameof(GetById), new { id = customer.Id }, response);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(CustomerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerResponse>> Update(
        Guid id,
        UpdateCustomerRequest request,
        CancellationToken cancellationToken)
    {
        var customer = await _customerService.UpdateAsync(
            id,
            request.FirstName,
            request.LastName,
            request.PhoneNumber,
            request.Email,
            cancellationToken);

        if (customer is null)
        {
            return NotFound();
        }

        return Ok(ToResponse(customer));
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var deleted = await _customerService.DeleteAsync(id, cancellationToken);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    private static CustomerResponse ToResponse(Customer customer)
    {
        return new CustomerResponse
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            PhoneNumber = customer.PhoneNumber,
            Email = customer.Email,
            CreatedAtUtc = customer.CreatedAtUtc
        };
    }
}
