using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OrderBook.Domain;

public static class WebApplicationBuilderExtensions
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderBookContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("OrderBookDb"));
        });

        return services;
    }
}