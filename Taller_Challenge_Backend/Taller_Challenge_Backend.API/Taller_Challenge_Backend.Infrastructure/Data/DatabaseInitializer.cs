using Taller_Challenge_Backend.Domain.Entities;
using Taller_Challenge_Backend.Domain.Enums;

namespace Taller_Challenge_Backend.Infrastructure.Data
{
    public class DatabaseInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Orders.Any()) return;

            var orders = new List<Order>();

            var order1 = Order.Create("Carlos Rodríguez", "1234-ABC", new List<OrderItem>
            {
                OrderItem.Create("Synthetic Oil Change", 1, 45.50m),
                OrderItem.Create("Oil filter", 1, 12.00m)
            });
            orders.Add(order1);

            var order2 = Order.Create("Ana Martínez", "9876-XYZ", new List<OrderItem>
            {
                OrderItem.Create("Front Brake Pads", 2, 35.00m),
                OrderItem.Create("Labor - Braking System", 1, 50.00m)
            });
            order2.UpdateStatus(OrderStatus.Pending);
            orders.Add(order2);

            var order3 = Order.Create("Roberto Gómez", "995-PLU", new List<OrderItem>
            {
                OrderItem.Create("12V LTH Battery", 1, 120.00m),
                OrderItem.Create("Computer Scanning", 1, 25.00m)
            });
            orders.Add(order3);

            var order4 = Order.Create("Lucía Fernández", "1122-DET", new List<OrderItem>
            {
                OrderItem.Create("Michelin Primacy tire", 4, 110.00m),
                OrderItem.Create("Alignment and Balancing", 1, 40.00m)
            });
            order4.UpdateStatus(OrderStatus.Completed);
            orders.Add(order4);

            var order5 = Order.Create("Marcos Soto", "3344-NPM", new List<OrderItem>
            {
                OrderItem.Create("Polishing and Waxing", 1, 85.00m),
                OrderItem.Create("Interior Cleaning", 1, 40.00m),
                OrderItem.Create("Tire change", 1, 120.00m)
            });
            orders.Add(order5);

            var order6 = Order.Create("Elena Torres", "1525-BLU", new List<OrderItem>
            {
                OrderItem.Create("Rear Shock Absorbers", 2, 95.00m),
                OrderItem.Create("Suspension Bushings", 4, 15.00m)
            });
            order6.UpdateStatus(OrderStatus.Approved);
            orders.Add(order6);

            var order7 = Order.Create("Daniel Vargas", "7788-TUV", new List<OrderItem>
            {
                OrderItem.Create("LED H7", 2, 22.50m),
                OrderItem.Create("Minor Electrical Installation", 1, 15.00m)
            });
            orders.Add(order7);

            context.Orders.AddRange(orders);
            context.SaveChanges();
        }
    }
}
