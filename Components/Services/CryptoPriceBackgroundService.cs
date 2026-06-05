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
            var nextMinute = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0, DateTimeKind.Utc)
                .AddMinutes(1);
            await Task.Delay(nextMinute - now, cancellationToken);

            await BackgroundFetchAsync();
        }
    }

    private async Task BackgroundFetchAsync()
    {
        using var scope = scopeFactory.CreateScope();
        var kraken = scope.ServiceProvider.GetRequiredService<KrakenService>();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        try
        {
            var prices = await kraken.FetchData();
            var timestamp = DateTime.UtcNow;

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