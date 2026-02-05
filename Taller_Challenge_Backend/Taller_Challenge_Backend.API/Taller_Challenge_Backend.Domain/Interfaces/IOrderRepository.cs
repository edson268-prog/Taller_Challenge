using Taller_Challenge_Backend.Domain.Entities;
using Taller_Challenge_Backend.Domain.Enums;

namespace Taller_Challenge_Backend.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default);
        Task<IEnumerable<Order>> GetWithPagingAsync(int page, int pageSize, CancellationToken cancellationToken = default);
        Task<Order> AddAsync(Order order, CancellationToken cancellationToken = default);
        Task UpdateAsync(Order order, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
