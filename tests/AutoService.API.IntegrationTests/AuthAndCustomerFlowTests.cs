using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace AutoService.API.IntegrationTests;

public sealed class AuthAndCustomerFlowTests : IClassFixture<AutoServiceApiFactory>
{
    private readonly HttpClient _client;

    public AuthAndCustomerFlowTests(AutoServiceApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task ProtectedCustomerFlow_RequiresAndAcceptsJwtToken()
    {
        var unauthorizedResponse = await _client.GetAsync("/api/customers");
        Assert.Equal(HttpStatusCode.Unauthorized, unauthorizedResponse.StatusCode);

        var setupResponse = await _client.PostAsJsonAsync("/api/auth/setup-admin", new
        {
            fullName = "Integration Admin",
            email = "integration-admin@example.com",
            password = "Integration123!"
        });
        setupResponse.EnsureSuccessStatusCode();

        using var setupJson = JsonDocument.Parse(await setupResponse.Content.ReadAsStringAsync());
        var token = setupJson.RootElement.GetProperty("accessToken").GetString();
        Assert.False(string.IsNullOrWhiteSpace(token));
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var createResponse = await _client.PostAsJsonAsync("/api/customers", new
        {
            firstName = "Integration",
            lastName = "Customer",
            phoneNumber = "+905551112233",
            email = "integration-customer@example.com"
        });
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var customersResponse = await _client.GetAsync("/api/customers");
        Assert.Equal(HttpStatusCode.OK, customersResponse.StatusCode);
        var customersJson = await customersResponse.Content.ReadAsStringAsync();
        Assert.Contains("integration-customer@example.com", customersJson);

        var invalidAppointmentResponse = await _client.PostAsJsonAsync("/api/appointments", new
        {
            vehicleId = Guid.NewGuid(),
            scheduledAtUtc = "2020-01-01T10:00:00Z",
            description = "Past appointment"
        });
        Assert.Equal(HttpStatusCode.BadRequest, invalidAppointmentResponse.StatusCode);
        var validationJson = await invalidAppointmentResponse.Content.ReadAsStringAsync();
        Assert.Contains("validation errors", validationJson, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("traceId", validationJson);
    }
}
