using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
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
/// Property-based tests for authorization invariant on tariff percentage endpoints.
/// **Validates: Requirements 9.1, 9.2**
///
/// Property 6: Authorization Invariant
/// For any request to a tariff percentage management endpoint from a user without
/// the System Administrator role, the system returns a 403 Forbidden response.
/// For any unauthenticated request, the system returns a 401 Unauthorized response.
/// </summary>
public class TariffPercentageAuthorizationPropertyTests : IDisposable
{
    private readonly WebApplicationFactory<Program> _nonAdminFactory;
    private readonly WebApplicationFactory<Program> _unauthenticatedFactory;

    /// <summary>
    /// Valid endpoint + method combinations based on the controller definition.
    /// </summary>
    private static readonly (string Path, HttpMethod Method)[] ValidEndpointCombinations = new[]
    {
        ("/api/admin/tariff-percentages", HttpMethod.Get),
        ("/api/admin/tariff-percentages", HttpMethod.Post),
        ("/api/admin/tariff-percentages/1", HttpMethod.Get),
        ("/api/admin/tariff-percentages/1", HttpMethod.Put),
        ("/api/admin/tariff-percentages/1", HttpMethod.Delete),
        ("/api/admin/tariff-percentages/1/apply", HttpMethod.Post),
        ("/api/admin/tariff-percentages/jobs/00000000-0000-0000-0000-000000000001", HttpMethod.Get),
        ("/api/admin/tariff-percentages/active-for-letter", HttpMethod.Get),
    };

    /// <summary>
    /// Roles that are NOT "System Administrator" to test 403 Forbidden.
    /// </summary>
    private static readonly string[] NonAdminRoles = new[]
    {
        "Case Manager",
        "Billing Auditing",
        "Metadata Administrator",
        "Imports",
        "Nurse",
        "Doctor",
        "ReadOnly",
        "User"
    };

    public TariffPercentageAuthorizationPropertyTests()
    {
        _nonAdminFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    // Override the default authentication scheme to our test scheme
                    services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = "TestScheme";
                        options.DefaultChallengeScheme = "TestScheme";
                        options.DefaultForbidScheme = "TestScheme";
                    })
                    .AddScheme<AuthenticationSchemeOptions, TestNonAdminAuthHandler>(
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
        _nonAdminFactory.Dispose();
        _unauthenticatedFactory.Dispose();
    }

    #region Custom Generators

    /// <summary>
    /// Generates arbitrary (endpoint, method) tuples from valid combinations.
    /// </summary>
    private static Arbitrary<(string Path, HttpMethod Method)> EndpointCombinationArbitrary()
    {
        var gen = Gen.Elements(ValidEndpointCombinations);
        return Arb.From(gen);
    }

    /// <summary>
    /// Generates arbitrary non-admin role names.
    /// </summary>
    private static Arbitrary<string> NonAdminRoleArbitrary()
    {
        var gen = Gen.Elements(NonAdminRoles);
        return Arb.From(gen);
    }

    #endregion

    /// <summary>
    /// Property: For any authenticated user without the "System Administrator" role,
    /// all requests to tariff percentage endpoints return 403 Forbidden.
    /// **Validates: Requirements 9.1, 9.2**
    /// </summary>
    [Property(MaxTest = 50)]
    public Property AuthenticatedNonAdminUser_AlwaysReceives403Forbidden()
    {
        return Prop.ForAll(
            EndpointCombinationArbitrary(),
            NonAdminRoleArbitrary(),
            (endpoint, role) =>
            {
                // Set the role for the auth handler to use
                TestNonAdminAuthHandler.CurrentRole = role;

                var client = _nonAdminFactory.CreateClient();

                var request = new HttpRequestMessage(endpoint.Method, endpoint.Path);

                // Add a minimal JSON body for POST/PUT requests to avoid 415 errors
                if (endpoint.Method == HttpMethod.Post || endpoint.Method == HttpMethod.Put)
                {
                    request.Content = new StringContent("{}", Encoding.UTF8, "application/json");
                }

                var response = client.SendAsync(request).GetAwaiter().GetResult();

                response.StatusCode.Should().Be(HttpStatusCode.Forbidden,
                    $"non-admin user with role '{role}' should be forbidden from {endpoint.Method} {endpoint.Path}");
            });
    }

    /// <summary>
    /// Property: For any unauthenticated request to tariff percentage endpoints,
    /// the system returns 401 Unauthorized.
    /// **Validates: Requirements 9.1, 9.2**
    /// </summary>
    [Property(MaxTest = 50)]
    public Property UnauthenticatedUser_AlwaysReceives401Unauthorized()
    {
        return Prop.ForAll(
            EndpointCombinationArbitrary(),
            endpoint =>
            {
                var client = _unauthenticatedFactory.CreateClient();

                var request = new HttpRequestMessage(endpoint.Method, endpoint.Path);

                // Add a minimal JSON body for POST/PUT requests
                if (endpoint.Method == HttpMethod.Post || endpoint.Method == HttpMethod.Put)
                {
                    request.Content = new StringContent("{}", Encoding.UTF8, "application/json");
                }

                var response = client.SendAsync(request).GetAwaiter().GetResult();

                response.StatusCode.Should().Be(HttpStatusCode.Unauthorized,
                    $"unauthenticated user should get 401 from {endpoint.Method} {endpoint.Path}");
            });
    }

    #region Test Infrastructure

    /// <summary>
    /// Authentication handler that simulates an authenticated user without System Administrator role.
    /// Uses a static CurrentRole property to allow the test to vary the role per invocation.
    /// </summary>
    public class TestNonAdminAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        /// <summary>
        /// The role to assign to the authenticated test user. Set this before making a request.
        /// </summary>
        public static string CurrentRole { get; set; } = "Case Manager";

        public TestNonAdminAuthHandler(
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
                new Claim(ClaimTypes.NameIdentifier, "test-user-id"),
                new Claim(ClaimTypes.Name, "testuser"),
                new Claim(ClaimTypes.Role, CurrentRole)
            };

            var identity = new ClaimsIdentity(claims, "TestScheme");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "TestScheme");

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }

    #endregion
}
