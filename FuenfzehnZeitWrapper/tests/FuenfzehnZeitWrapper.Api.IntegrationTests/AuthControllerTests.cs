using FuenfzehnZeitWrapper.Api.IntegrationTests.Fixtures;
using FuenfzehnZeitWrapper.Application.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using System.Net;
using System.Net.Http.Json;

namespace FuenfzehnZeitWrapper.Api.IntegrationTests;

public sealed class AuthControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public AuthControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task LogIn_ReturnsProblemDetails_WhenExceptionThrown()
    {
        // Arrange
        var fzServiceMock = Substitute.For<IFuenfzehnZeitService>();
        fzServiceMock.LogInAsync().Returns(Task.FromException(new Exception()));

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<IFuenfzehnZeitService>();
                services.AddScoped<IFuenfzehnZeitService>(_ => fzServiceMock);
            });
        }).CreateClient();

        // Act
        var response = await client.GetAsync("api/v1/auth/log-in");


        // Assert
        Assert.NotNull(response);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task LogIn_ReturnsProblemDetails_WhenNonExistingEndpoint()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("non-existing-endpoint");
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        // Assert
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal(404, problem?.Status);
    }
}
