using Application.Repositories;
using Domain.Common;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : Entity
    {
        protected readonly DataContext _context;

        public BaseRepository(DataContext context)
        {
            _context = context;
        }

        public void Create(T entity)
        {
            _context.Add(entity);
        }

        public async Task<EntityEntry<T>> CreateAsync(T entity, CancellationToken cancellationToken)
        {
            return await _context.AddAsync(entity, cancellationToken);
        }

        public void Update(T entity)
        {
            _context.Update(entity);
        }

        public void HardDelete(T entity)
        {
            _context.Remove(entity);
        }

        public void SoftDelete(T entity)
        {
            entity.DateDeleted = DateTime.Now;
            _context.Update(entity);
        }

        public T? Get(Guid id)
        {
            return _context.Set<T>().FirstOrDefault(x => x.Id == id);
        }

        public async Task<T?> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<T>().ToListAsync(cancellationToken);
        }
    }
}
