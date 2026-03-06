using FuenfzehnZeitWrapper.Presentation.IntegrationTests.Fixtures;
using FuenfzehnZeitWrapper.Presentation.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Options;
using System.Net;

namespace FuenfzehnZeitWrapper.Presentation.IntegrationTests;

public sealed class AuthControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public AuthControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task LogIn_Returns200_WhenSucess()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("api/v1/auth/log-in");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
