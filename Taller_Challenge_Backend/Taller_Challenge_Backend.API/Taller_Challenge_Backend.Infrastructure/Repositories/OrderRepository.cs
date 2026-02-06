using Microsoft.EntityFrameworkCore;
using Taller_Challenge_Backend.Domain.Entities;
using Taller_Challenge_Backend.Domain.Enums;
using Taller_Challenge_Backend.Domain.Interfaces;
using Taller_Challenge_Backend.Infrastructure.Data;

namespace Taller_Challenge_Backend.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Order>> GetFilteredOrdersAsync(OrderStatus? status, int page, int pageSize, string? sortOrder, CancellationToken cancellationToken = default)
        {
            var query = _context.Orders.Include(o => o.Items).AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(o => o.Status == status.Value);
            }

            query = sortOrder?.ToLower() switch
            {
                "asc" => query.OrderBy(o => o.CreatedAt),
                "status" => query.OrderBy(o => o.Status),
                _ => query.OrderByDescending(o => o.CreatedAt)
            };

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<Order> AddAsync(Order order, CancellationToken cancellationToken = default)
        {
            await _context.Orders.AddAsync(order, cancellationToken);
            return order;
        }

        public Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
        {
            _context.Entry(order).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
