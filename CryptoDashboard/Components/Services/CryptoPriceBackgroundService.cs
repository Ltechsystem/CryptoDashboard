using CryptoDashboard.Components.Models;

namespace CryptoDashboard.Components.Services;

public class CryptoPriceBackgroundService(
    IServiceScopeFactory scopeFactory,
    ILogger<CryptoPriceBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;
            var nextTick = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute,
                now.Second - (now.Second % 30), DateTimeKind.Utc).AddSeconds(30);
            await Task.Delay(nextTick - now, cancellationToken);

            await BackgroundFetchAsync(nextTick);
        }
    }

    private async Task BackgroundFetchAsync(DateTime tickUtc)
    {
        using var scope = scopeFactory.CreateScope();
        var kraken = scope.ServiceProvider.GetRequiredService<KrakenService>();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        try
        {
            var prices = await kraken.FetchData();
            var tz = TimeZoneInfo.FindSystemTimeZoneById("Europe/Copenhagen");
            var timestamp = TimeZoneInfo.ConvertTimeFromUtc(tickUtc, tz);

            foreach (var (symbol, price) in prices)
            {
                db.Snapshots.Add(new CryptoDataModel
                {
                    Symbol = symbol,
                    Price = price,
                    Timestamp = timestamp,
                });
            }

            await db.SaveChangesAsync();
            logger.LogInformation("Saved {Count} snapshots at {Timestamp}", prices.Count, timestamp);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to fetch crypto prices");
        }
    }
}