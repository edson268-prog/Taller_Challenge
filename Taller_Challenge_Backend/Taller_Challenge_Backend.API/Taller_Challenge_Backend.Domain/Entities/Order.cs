using Taller_Challenge_Backend.Domain.Enums;

namespace Taller_Challenge_Backend.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; private set; }
        public string CustomerName { get; private set; }
        public string VehiclePlate { get; private set; }
        public OrderStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public decimal Subtotal { get; private set; }
        public decimal TaxAmount { get; private set; }
        public decimal DiscountAmount { get; private set; }
        public decimal TotalAmount { get; private set; }

        public ICollection<OrderItem> Items { get; private set; } = new List<OrderItem>();

        // Factory Method to encapsulate validation and creation logic, ensuring the object is always in a valid state.
        public static Order Create(string customerName, string vehiclePlate, List<OrderItem> items)
        {
            if (string.IsNullOrWhiteSpace(customerName))
                throw new ArgumentException("Customer name is required", nameof(customerName));

            if (string.IsNullOrWhiteSpace(vehiclePlate))
                throw new ArgumentException("Vehicle plate is required", nameof(vehiclePlate));

            if (items == null || !items.Any())
                throw new ArgumentException("Order must have at least one item", nameof(items));

            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerName = customerName.Trim(),
                VehiclePlate = vehiclePlate.Trim().ToUpper(),
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                Items = items
            };

            order.CalculateSubtotal();
            return order;
        }

        public void CalculateSubtotal()
        {
            Subtotal = Items.Sum(item => item.Quantity * item.UnitPrice);
        }

        public void ApplyPricing(decimal taxAmount, decimal discountAmount)
        {
            TaxAmount = taxAmount;
            DiscountAmount = discountAmount;
            TotalAmount = Subtotal + TaxAmount - DiscountAmount;
        }

        public void UpdateStatus(OrderStatus status)
        {
            Status = status;
        }

        private Order() { } // Empty constructor for EF Core
    }

    public class OrderItem
    {
        public Guid Id { get; private set; }
        public Guid OrderId { get; private set; }
        public string Description { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        public static OrderItem Create(string description, int quantity, decimal unitPrice)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description is required", nameof(description));

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than 0", nameof(quantity));

            if (unitPrice <= 0)
                throw new ArgumentException("Unit price must be greater than 0", nameof(unitPrice));

            return new OrderItem
            {
                Id = Guid.NewGuid(),
                Description = description.Trim(),
                Quantity = quantity,
                UnitPrice = unitPrice
            };
        }

        private OrderItem() { } // Empty constructor for EF Core
    }
}
