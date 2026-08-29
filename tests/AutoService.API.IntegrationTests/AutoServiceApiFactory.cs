using AutoService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AutoService.API.IntegrationTests;

public sealed class AutoServiceApiFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = $"autoservice-tests-{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.UseSetting(
            "ConnectionStrings:PostgreSql",
            "Host=localhost;Database=unused;Username=unused;Password=unused");
        builder.UseSetting("Jwt:Issuer", "AutoService.API.Tests");
        builder.UseSetting("Jwt:Audience", "AutoService.Tests");
        builder.UseSetting("Jwt:Key", "integration-test-key-with-at-least-thirty-two-bytes");
        builder.UseSetting("Jwt:ExpiresInMinutes", "10");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<AutoServiceDbContext>>();
            services.RemoveAll<IDbContextOptionsConfiguration<AutoServiceDbContext>>();
            services.AddDbContext<AutoServiceDbContext>(options =>
                options.UseInMemoryDatabase(_databaseName));
        });
    }
}
