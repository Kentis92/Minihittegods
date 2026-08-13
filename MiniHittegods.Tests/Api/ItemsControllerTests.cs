using MiniHittegods.Domain.Entities;
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace MiniHittegods.Tests.Api;

public class ItemsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ItemsControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Post_item_should_return_201_created()
    {
        var request = new
        {
            title = "Blå jakke",
            description = "Norge skrevet på ryggen",
            category = "Clothing",
            foundLocation = "Scene 2"
        };

        var response = await _client.PostAsJsonAsync(
            "/api/items",
            request);

        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);
    }

    [Fact]
    public async Task Post_item_without_title_should_return_400_bad_request()
    {
        var request = new
        {
            title = "",
            description = "Norge skrevet på ryggen",
            category = "Clothing",
            foundLocation = "Scene 2"
        };

        var response = await _client.PostAsJsonAsync(
            "/api/items",
            request);

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task Get_items_should_return_200_ok()
    {
        var response = await _client.GetAsync("/api/items");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task Get_item_by_id_should_return_200_ok()
    {
        var request = new
        {
            title = "Svart lommebok",
            description = "Fant ved inngangen",
            category = "Wallet",
            foundLocation = "Inngang A"
        };

        var createResponse = await _client.PostAsJsonAsync(
            "/api/items",
            request);

        var location = createResponse.Headers.Location!.ToString();

        var response = await _client.GetAsync(location);

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task Claim_item_should_return_200_ok()
    {
        var createRequest = new
        {
            title = "Rød sekk",
            description = "Fant ved scenen",
            category = "Other",
            foundLocation = "Scene 1"
        };

        var createResponse = await _client.PostAsJsonAsync(
            "/api/items",
            createRequest);

        var location = createResponse.Headers.Location!.ToString();

        var id = location.Split('/').Last();

        var claimRequest = new
        {
            claimedBy = "Ola Nordmann"
        };

        var response = await _client.PostAsJsonAsync(
            $"/api/items/{id}/claim",
            claimRequest);

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task Return_item_should_return_200_ok()
    {
        var createRequest = new
        {
            title = "Mobil",
            description = "Sort iPhone",
            category = "Electronics",
            foundLocation = "Scene 3"
        };

        var createResponse = await _client.PostAsJsonAsync(
            "/api/items",
            createRequest);

        var location = createResponse.Headers.Location!.ToString();

        var id = location.Split('/').Last();

        await _client.PostAsJsonAsync(
            $"/api/items/{id}/claim",
            new
            {
                claimedBy = "Ola Nordmann"
            });

        var response = await _client.PostAsync(
            $"/api/items/{id}/return",
            null);

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task Delete_item_should_return_204_no_content()
    {
        var createRequest = new
        {
            title = "Nøkler",
            description = "Bilnøkler",
            category = "Keys",
            foundLocation = "Inngang"
        };

        var createResponse = await _client.PostAsJsonAsync(
            "/api/items",
            createRequest);

        var location = createResponse.Headers.Location!.ToString();

        var response = await _client.DeleteAsync(location);

        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);
    }

    [Fact]
    public async Task Delete_claimed_item_should_return_409_conflict()
    {
        var createRequest = new
        {
            title = "Telefon",
            description = "Svart mobil",
            category = "Electronics",
            foundLocation = "Scene 4"
        };

        var createResponse = await _client.PostAsJsonAsync(
            "/api/items",
            createRequest);

        var location = createResponse.Headers.Location!.ToString();

        var id = location.Split('/').Last();

        await _client.PostAsJsonAsync(
            $"/api/items/{id}/claim",
            new
            {
                claimedBy = "Ola Nordmann"
            });

        var response = await _client.DeleteAsync(
            location);

        Assert.Equal(
            HttpStatusCode.Conflict,
            response.StatusCode);
    }

    [Fact]
    public async Task Claim_already_claimed_item_should_return_409_conflict()
    {
        var createRequest = new
        {
            title = "PC",
            description = "Gaming laptop",
            category = "Electronics",
            foundLocation = "Scene 5"
        };

        var createResponse = await _client.PostAsJsonAsync(
            "/api/items",
            createRequest);

        var location = createResponse.Headers.Location!.ToString();

        var id = location.Split('/').Last();

        await _client.PostAsJsonAsync(
            $"/api/items/{id}/claim",
            new
            {
                claimedBy = "Ola Nordmann"
            });

        var response = await _client.PostAsJsonAsync(
            $"/api/items/{id}/claim",
            new
            {
                claimedBy = "Kari Nordmann"
            });

        Assert.Equal(
            HttpStatusCode.Conflict,
            response.StatusCode);
    }
}