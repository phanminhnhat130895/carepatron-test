using Domain.Common;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Application.Repositories
{
    public interface IBaseRepository<T> where T : Entity
    {
        void Create(T entity);
        Task<EntityEntry<T>> CreateAsync(T entity, CancellationToken cancellationToken);
        void Update(T entity);
        void SoftDelete(T entity);
        void HardDelete(T entity);
        T? Get(Guid id);
        Task<T?> GetAsync(Guid id, CancellationToken cancellationToken);
        List<T> GetAll();
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken);
    }
}
