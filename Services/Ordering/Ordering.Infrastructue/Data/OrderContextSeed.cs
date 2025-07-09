using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructue.Data
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation($"Ordering Database: {typeof(OrderContext).Name} seedes");
            }
        }

        private static IEnumerable<Order> GetOrders()
        {
            return new List<Order>
            {
                new()
                {
                    UserName = "thai",
                    FirstName = "Thai",
                    LastName = "Dinh Xuan",
                    EmailAddress = "xuanthai@eCommerce.net",
                    AddressLine = "123 Hồ Đắc Di",
                    Country = "VietNam",
                    TotalPrice = 750,
                    State = "Hue city",
                    ZipCode = "730006",

                    CardName = "Visa",
                    CardNumber = "1234567890123456",
                    CreatedBy = "Thai",
                    Expiration = "12/25",
                    Cvv = "123",
                    PaymentMethod = 1,
                    LastModifiedBy = "Thai",
                    LastModifiedDate = new DateTime(),
                }
            };
        }
    }
}
