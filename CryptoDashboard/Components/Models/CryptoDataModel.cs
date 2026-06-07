namespace CryptoDashboard.Components.Models;

public class CryptoDataModel
{
    public int Id { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime Timestamp { get; set; }
}