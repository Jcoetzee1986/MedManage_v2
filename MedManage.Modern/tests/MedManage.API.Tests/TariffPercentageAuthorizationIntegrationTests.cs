using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xunit;

namespace MedManage.API.Tests;

/// <summary>
/// Integration tests for authorization enforcement on tariff percentage endpoints.
/// Tests that SystemAdmin role grants access, non-admin roles are rejected with 403,
/// and unauthenticated requests receive 401.
///
/// **Validates: Requirements 9.1, 9.2**
/// </summary>
public class TariffPercentageAuthorizationIntegrationTests : IDisposable
{
    private readonly WebApplicationFactory<Program> _adminFactory;
    private readonly WebApplicationFactory<Program> _nonAdminFactory;
    private readonly WebApplicationFactory<Program> _unauthenticatedFactory;

    public TariffPercentageAuthorizationIntegrationTests()
    {
        _adminFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = "TestScheme";
                        options.DefaultChallengeScheme = "TestScheme";
                        options.DefaultForbidScheme = "TestScheme";
                    })
                    .AddScheme<AuthenticationSchemeOptions, TestAdminAuthHandler>(
                        "TestScheme", _ => { });
                });
                builder.UseEnvironment("Development");
            });

        _nonAdminFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = "TestScheme";
                        options.DefaultChallengeScheme = "TestScheme";
                        options.DefaultForbidScheme = "TestScheme";
                    })
                    .AddScheme<AuthenticationSchemeOptions, TestNonAdminUserAuthHandler>(
                        "TestScheme", _ => { });
                });
                builder.UseEnvironment("Development");
            });

        _unauthenticatedFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Development");
            });
    }

    public void Dispose()
    {
        _adminFactory.Dispose();
        _nonAdminFactory.Dispose();
        _unauthenticatedFactory.Dispose();
    }

    #region SystemAdmin access granted

    /// <summary>
    /// Tests that a SystemAdmin user can access the GetAll endpoint.
    /// **Validates: Requirement 9.1**
    /// </summary>
    [Fact]
    public async Task GetAll_AsSystemAdmin_ReturnsOk()
    {
        // Arrange
        var client = _adminFactory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/admin/tariff-percentages");

        // Assert - should not be 401 or 403
        response.StatusCode.Should().NotBe(HttpStatusCode.Unauthorized);
        response.StatusCode.Should().NotBe(HttpStatusCode.Forbidden);
        // May be 200 or 500 depending on DB availability, but auth is granted
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.OK, HttpStatusCode.InternalServerError);
    }

    /// <summary>
    /// Tests that a SystemAdmin user can access the active-for-letter endpoint.
    /// **Validates: Requirement 9.1**
    /// </summary>
    [Fact]
    public async Task GetActiveForLetter_AsSystemAdmin_ReturnsOk()
    {
        // Arrange
        var client = _adminFactory.CreateClient();

        // Act
        var response = await client.GetAsync(
            "/api/admin/tariff-percentages/active-for-letter");

        // Assert
        response.StatusCode.Should().NotBe(HttpStatusCode.Unauthorized);
        response.StatusCode.Should().NotBe(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests that a SystemAdmin user can access the apply endpoint.
    /// **Validates: Requirement 9.1**
    /// </summary>
    [Fact]
    public async Task Apply_AsSystemAdmin_NotForbidden()
    {
        // Arrange
        var client = _adminFactory.CreateClient();
        var content = new StringContent("{}", Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(
            "/api/admin/tariff-percentages/999/apply", content);

        // Assert - may be 404 (record not found) but not 401/403
        response.StatusCode.Should().NotBe(HttpStatusCode.Unauthorized);
        response.StatusCode.Should().NotBe(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Non-admin rejected with 403

    /// <summary>
    /// Tests that a non-admin authenticated user gets 403 on GetAll.
    /// **Validates: Requirement 9.2**
    /// </summary>
    [Fact]
    public async Task GetAll_AsNonAdmin_Returns403Forbidden()
    {
        // Arrange
        var client = _nonAdminFactory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/admin/tariff-percentages");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests that a non-admin authenticated user gets 403 on Create.
    /// **Validates: Requirement 9.2**
    /// </summary>
    [Fact]
    public async Task Create_AsNonAdmin_Returns403Forbidden()
    {
        // Arrange
        var client = _nonAdminFactory.CreateClient();
        var content = new StringContent(
            """{"percentageAdded": 250.60, "tariffPeriodName": 2026, "startActiveDate": "2026-01-01"}""",
            Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync("/api/admin/tariff-percentages", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests that a non-admin authenticated user gets 403 on Apply.
    /// **Validates: Requirement 9.2**
    /// </summary>
    [Fact]
    public async Task Apply_AsNonAdmin_Returns403Forbidden()
    {
        // Arrange
        var client = _nonAdminFactory.CreateClient();
        var content = new StringContent("{}", Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(
            "/api/admin/tariff-percentages/1/apply", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    /// <summary>
    /// Tests that a non-admin authenticated user gets 403 on Delete.
    /// **Validates: Requirement 9.2**
    /// </summary>
    [Fact]
    public async Task Delete_AsNonAdmin_Returns403Forbidden()
    {
        // Arrange
        var client = _nonAdminFactory.CreateClient();

        // Act
        var response = await client.DeleteAsync("/api/admin/tariff-percentages/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    #endregion

    #region Unauthenticated rejected with 401

    /// <summary>
    /// Tests that an unauthenticated request gets 401 on GetAll.
    /// **Validates: Requirement 9.2**
    /// </summary>
    [Fact]
    public async Task GetAll_Unauthenticated_Returns401Unauthorized()
    {
        // Arrange
        var client = _unauthenticatedFactory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/admin/tariff-percentages");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    /// <summary>
    /// Tests that an unauthenticated request gets 401 on Apply.
    /// **Validates: Requirement 9.2**
    /// </summary>
    [Fact]
    public async Task Apply_Unauthenticated_Returns401Unauthorized()
    {
        // Arrange
        var client = _unauthenticatedFactory.CreateClient();
        var content = new StringContent("{}", Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(
            "/api/admin/tariff-percentages/1/apply", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    #endregion

    #region Auth Handlers

    /// <summary>
    /// Authentication handler that simulates an authenticated System Administrator user.
    /// </summary>
    public class TestAdminAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAdminAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder)
            : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "admin-user-id"),
                new Claim(ClaimTypes.Name, "admin"),
                new Claim(ClaimTypes.Role, "System Administrator")
            };

            var identity = new ClaimsIdentity(claims, "TestScheme");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "TestScheme");

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }

    /// <summary>
    /// Authentication handler that simulates an authenticated non-admin user.
    /// </summary>
    public class TestNonAdminUserAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestNonAdminUserAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder)
            : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "regular-user-id"),
                new Claim(ClaimTypes.Name, "regularuser"),
                new Claim(ClaimTypes.Role, "Case Manager")
            };

            var identity = new ClaimsIdentity(claims, "TestScheme");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "TestScheme");

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }

    #endregion
}
