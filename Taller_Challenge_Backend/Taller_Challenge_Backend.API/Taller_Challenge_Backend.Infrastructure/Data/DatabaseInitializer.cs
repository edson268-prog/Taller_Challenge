using Taller_Challenge_Backend.Domain.Entities;
using Taller_Challenge_Backend.Domain.Enums;

namespace Taller_Challenge_Backend.Infrastructure.Data
{
    public class DatabaseInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {
                var users = new List<User>
                {
                    User.Create("eibanez268", "ZXhzcXVhcmVkYWRtaW4xMjM=", "eibanez@exsquared.com", Role.Admin), // exsquaredadmin123
                    User.Create("amonrroy151", "aWFtYXZpc2l0b3I0NTY=", "amonrroy@exsquared.com", Role.Visitor)  // iamavisitor456
                };

                context.Users.AddRange(users);
                context.SaveChanges();
            }

            if (!context.Orders.Any())
            {
                var orders = new List<Order>
                {
                    Order.Create("Carlos Rodríguez", "1234-ABC", new List<OrderItem>
                    {
                        OrderItem.Create("Synthetic Oil Change", 1, 45.50m),
                        OrderItem.Create("Oil filter", 1, 12.00m)
                    }),

                    Order.Create("Ana Martínez", "9876-XYZ", new List<OrderItem>
                    {
                        OrderItem.Create("Front Brake Pads", 2, 35.00m),
                        OrderItem.Create("Labor - Braking System", 1, 50.00m)
                    }),

                    Order.Create("Roberto Gómez", "995-PLU", new List<OrderItem>
                    {
                        OrderItem.Create("12V LTH Battery", 1, 120.00m),
                        OrderItem.Create("Computer Scanning", 1, 25.00m)
                    }),

                    Order.Create("Lucía Fernández", "1122-DET", new List<OrderItem>
                    {
                        OrderItem.Create("Michelin Primacy tire", 4, 110.00m),
                        OrderItem.Create("Alignment and Balancing", 1, 40.00m)
                    }),

                    Order.Create("Marcos Soto", "3344-NPM", new List<OrderItem>
                    {
                        OrderItem.Create("Polishing and Waxing", 1, 85.00m),
                        OrderItem.Create("Interior Cleaning", 1, 40.00m),
                        OrderItem.Create("Tire change", 1, 120.00m)
                    }),

                    Order.Create("Elena Torres", "1525-BLU", new List<OrderItem>
                    {
                        OrderItem.Create("Rear Shock Absorbers", 2, 95.00m),
                        OrderItem.Create("Suspension Bushings", 4, 15.00m)
                    }),

                    Order.Create("Daniel Vargas", "7788-TUV", new List<OrderItem>
                    {
                        OrderItem.Create("LED H7", 2, 22.50m),
                        OrderItem.Create("Minor Electrical Installation", 1, 15.00m)
                    }),

                    Order.Create("Maria Valenzuela", "1845-CLE", new List<OrderItem>
                    {
                        OrderItem.Create("Emergency breaks", 1, 500.00m),
                        OrderItem.Create("360-Degree Camera", 1, 150.00m)
                    })
                };

                orders[3].UpdateStatus(OrderStatus.Completed);
                orders[5].UpdateStatus(OrderStatus.Approved);

                context.Orders.AddRange(orders);
                context.SaveChanges();
            }
        }
    }
}
