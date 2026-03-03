using CapStone.Application.Repositories;
using CapStone.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CapStone.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AccidentDbContext _context;
        private readonly DbSet<T> _set;

        public Repository(AccidentDbContext context)
        {
            _context = context;
            _set = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _set.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _set.ToListAsync(cancellationToken);
        }

        public IQueryable<T> GetQueryable()
        {
            return _set.AsQueryable();
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _set.AddAsync(entity, cancellationToken);
            return entity;
        }

        public void Update(T entity)
        {
            _set.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Remove(T entity)
        {
            _set.Remove(entity);
        }
    }
}
