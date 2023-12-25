using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext context,ILogger<OrderContextSeed> logger)
        {
            if(!context.Orders.Any())
            {
                context.Orders.AddRange(GetPreconfigureOrders());
                await context.SaveChangesAsync();
                logger.LogInformation("seed database associated with context {DbContextName}",typeof(OrderContext));
            }
        }

        public static IEnumerable<Order> GetPreconfigureOrders()
        {
            return new List<Order>()
            {
                new Order() {UserName="swn", FirstName="Mehmet", LastName="Ozkaya",EmailAddress="abc@gmail.com", AddressLine="Pune",TotalPrice= 300,CardName="Visa",CardNumber="1234"
                ,Country="IND",CVV="123",Expiration="11/20",State="MH",PaymentMethod=1,ZipCode="411058",LastModifiedBy="1",CreatedBy="1"
                }
            };
        }
    }
}
