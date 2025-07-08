
using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using Ordering.Infrastructue.Data;

namespace Ordering.Infrastructue.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {

        public OrderRepository(OrderContext dbContext) : base(dbContext)
        {

        }
        public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
        {
            var orderList = await _dbContext.Orders
                .Where(o => o.UserName == userName)
                .ToListAsync();
            return orderList;
        }
    }
}
