using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using LibraryDDD.Api.Endpoints;
using LibraryDDD.Api.Requests;
using LibraryDDD.Application;
using LibraryDDD.Application.Common.Interfaces;
using LibraryDDD.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LibraryDDD.API.Tests.Endpoints;

public class MemberEndpointsTests
{
    [Fact]
    public async Task RegisterMember_WithValidRequest_ReturnsMemberIdAndPersistsMember()
    {
        await using var host = await EndpointTestHost.CreateAsync();

        var response = await host.Client.PostAsJsonAsync(
            "/members/register",
            CreateValidRequest());

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseBody = await response.Content.ReadFromJsonAsync<JsonElement>();
        var memberId = responseBody.GetProperty("memberId").GetGuid();
        Assert.NotEqual(Guid.Empty, memberId);
    }

    [Fact]
    public async Task RegisterMember_WithInvalidRequest_ReturnsValidationProblemWithoutSaving()
    {
        await using var host = await EndpointTestHost.CreateAsync();
        var request = CreateValidRequest() with
        {
            Name = "",
            DateOfBirth = "not-a-date",
            MembershipType = ""
        };

        var response = await host.Client.PostAsJsonAsync("/members/register", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        using var responseBody = await JsonDocument.ParseAsync(
            await response.Content.ReadAsStreamAsync());
        var errors = responseBody.RootElement.GetProperty("errors");
        Assert.True(errors.TryGetProperty("Name", out _));
        Assert.True(errors.TryGetProperty("DateOfBirth", out _));
        Assert.True(errors.TryGetProperty("MembershipType", out _));
    }

    [Fact]
    public async Task RegisterMember_WhenHandlerValidationFails_ReturnsValidationProblemWithoutSaving()
    {
        await using var host = await EndpointTestHost.CreateAsync();
        var request = CreateValidRequest() with { Email = "not-an-email" };

        var response = await host.Client.PostAsJsonAsync("/members/register", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        using var responseBody = await JsonDocument.ParseAsync(
            await response.Content.ReadAsStreamAsync());
        var errors = responseBody.RootElement.GetProperty("errors");
        Assert.True(errors.TryGetProperty("Email.InvalidFormat", out _));
    }

    [Fact]
    public async Task RegisterMember_WhenUsingWrongHttpMethod_ReturnsMethodNotAllowed()
    {
        await using var host = await EndpointTestHost.CreateAsync();

        var response = await host.Client.GetAsync("/members/register");

        Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
    }

    [Fact]
    public async Task RegisterMember_WhenUsingUnknownRoute_ReturnsNotFound()
    {
        await using var host = await EndpointTestHost.CreateAsync();

        var response = await host.Client.PostAsJsonAsync(
            "/members/unknown",
            CreateValidRequest());

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private static RegisterMemberRequest CreateValidRequest() =>
        new(
            Name: "Alex Example",
            PhoneNumber: null,
            Email: "alex@example.com",
            DateOfBirth: "1990-01-01",
            MembershipType: "Adult",
            StaffId: null,
            Street: null,
            City: null,
            PostalCode: null,
            Country: null);

    private sealed class EndpointTestHost : IAsyncDisposable
    {
        private readonly WebApplication _application;

        private EndpointTestHost(
            WebApplication application,
            HttpClient client)
        {
            _application = application;
            Client = client;
        }

        public HttpClient Client { get; }

        public static async Task<EndpointTestHost> CreateAsync()
        {
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                EnvironmentName = Environments.Development
            });
            builder.WebHost.UseUrls("http://127.0.0.1:0");
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure();


            var application = builder.Build();
            application.MapMemberEndpoints();
            await application.StartAsync();

            var server      = application.Services.GetRequiredService<IServer>();

            var address = server.Features
                .Get<IServerAddressesFeature>()!
                .Addresses
                .Single();
            var client = new HttpClient { BaseAddress = new Uri(address) };

            return new EndpointTestHost(application, client);
        }

        public async ValueTask DisposeAsync()
        {
            Client.Dispose();
            await _application.DisposeAsync();
        }
    }

}
