using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace OrderBook.Domain;

public class OrderBookContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public OrderBookContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to sql server with connection string from app settings
        options.UseSqlServer(Configuration.GetConnectionString("OrderBookDb"));
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<MetaExchange> MetaExchanges { get; set; }
}