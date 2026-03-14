using Ai_ShopBot.Croe.Interfaces;
using Ai_ShopBot.Presistance.Context;

namespace Ai_ShopBot.Presistance
{
    internal class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ShopDbContext _context;

        public BaseRepository(ShopDbContext context)
        {
            _context = context;
        }

        public BaseRepository()
        {
            
        }

        public virtual IQueryable<T> Entities => _context.Set<T>();

        public virtual async Task AddAsync(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            await _context.AddAsync(entity);
        }

        public virtual async Task AddRange(IEnumerable<T> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);

            await _context.AddRangeAsync(entities);
        }

        public virtual void Delete(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _context.Remove(entity);
        }

        public virtual void Update(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _context.Update(entity);
        }

        public virtual void UpdateRange(IEnumerable<T> entities)
        {
            _context.UpdateRange(entities);
        }
    }
}
