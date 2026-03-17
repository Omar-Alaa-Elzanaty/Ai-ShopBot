using System;
using System.Collections.Generic;
using System.Text;

namespace Ai_ShopBot.Croe.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task AddRange(IEnumerable<T> entities);
        void Delete(T entity);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
    }
}
