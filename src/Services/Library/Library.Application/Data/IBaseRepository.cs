using Domain.Abstractions.Abstractions;

namespace Library.Application.Data
{
    public interface IBaseRepository<T, TId> where T : Aggregate<TId>
    {
        Task AddAsync(T entity, CancellationToken cancellationToken);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
