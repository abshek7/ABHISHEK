using CapStone.Application.Repositories;
using CapStone.Infrastructure.Data;

namespace CapStone.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AccidentDbContext _context;

        public UnitOfWork(AccidentDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
