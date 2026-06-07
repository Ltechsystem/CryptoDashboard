using System.Net;
using CryptoDashboard.Components.Services;

namespace CryptoDashboard.Tests;

public class KrakenServiceIntegrationTests
{
    private static KrakenService CreateService()
    {
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.kraken.com/"),
        };
        return new KrakenService(httpClient);
    }

    [Fact]
    public async Task CanConnectToKrakenApi()
    {
        using var httpClient = new HttpClient { BaseAddress = new Uri("https://api.kraken.com/") };

        var response = await httpClient.GetAsync("0/public/Time");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task FetchData_ReturnsPrices()
    {
        var service = CreateService();

        var prices = await service.FetchData();

        Assert.NotEmpty(prices);
    }
}