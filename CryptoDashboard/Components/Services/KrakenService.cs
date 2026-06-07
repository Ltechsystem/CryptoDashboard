using System.Globalization;
using System.Text.Json.Serialization;

namespace CryptoDashboard.Components.Services;

public class KrakenService
{
    private static readonly Dictionary<string, string> _pairs = new()
    {
        ["BTC/USD"] = "XXBTZUSD",
        ["ETH/USD"] = "XETHZUSD",
        ["XRP/USD"] = "XXRPZUSD",
        ["ADA/USD"] = "ADAUSD",
        ["SOL/USD"] = "SOLUSD",
    };

    private readonly HttpClient _httpClient;

    public KrakenService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Dictionary<string, decimal>> FetchData()
    {
        var pairQuery = string.Join(",", _pairs.Values);
        var response = await _httpClient.GetFromJsonAsync<KrakenTickerResponse>(
            $"0/public/Ticker?pair={pairQuery}");

        if (response?.Result is null)
            return [];

        var result = new Dictionary<string, decimal>();
        foreach (var (displayName, krakenPair) in _pairs)
        {
            if (response.Result.TryGetValue(krakenPair, out var tickerData) &&
                tickerData.LastTrade is { Length: > 0 } &&
                decimal.TryParse(tickerData.LastTrade[0], NumberStyles.Any, CultureInfo.InvariantCulture, out var price))
            {
                result[displayName] = price;
            }
        }

        return result;
    }
}

public class KrakenTickerResponse
{
    [JsonPropertyName("error")]
    public string[] Error { get; set; } = [];

    [JsonPropertyName("result")]
    public Dictionary<string, KrakenTickerData>? Result { get; set; }
}

public class KrakenTickerData
{
    [JsonPropertyName("c")]
    public string[] LastTrade { get; set; } = [];
}