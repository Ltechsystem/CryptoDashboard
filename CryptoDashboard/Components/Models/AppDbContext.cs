using Microsoft.EntityFrameworkCore;

namespace CryptoDashboard.Components.Models;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<CryptoDataModel> Snapshots => Set<CryptoDataModel>();
}