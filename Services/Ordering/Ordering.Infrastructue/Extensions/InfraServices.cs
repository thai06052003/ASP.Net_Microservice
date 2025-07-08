using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Core.Repositories;
using Ordering.Infrastructue.Data;
using Ordering.Infrastructue.Repositories;

namespace Ordering.Infrastructure.Extensions
{
    public static class InfraServices
    {
        public static IServiceCollection AddInfraServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddDbContext<OrderContext>(options => options.UseSqlServer(
                configuration.GetConnectionString("OrderingConnectionString"),
                sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));
            serviceCollection.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
            serviceCollection.AddScoped<IOrderRepository, OrderRepository>();
            return serviceCollection;
        }
    }
}