using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TakeMe.Core.Entities;
using TakeMe.Core.Interfaces;
using TakeMe.InferStructuer.Data;

namespace TakeMe.InferStructuer.Repositries
{
    public class GenericRepositrie<T> : IGenericRepositrie<T> where T : BaseEntity<int>
    {
        private readonly ApplicationDbContext context;

        public GenericRepositrie(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(T entity)
        {
            await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task<int> CountAsync()
        => await context.Set<T>().CountAsync();

        public async Task DeleteAsync(int id)
        {
            var entity = await context.Set<T>().FindAsync(id);
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll()
        => context.Set<T>().AsNoTracking().ToList();

        

        public async Task<IReadOnlyList<T>> GetAllAsync()
        => await context.Set<T>().AsNoTracking().ToListAsync();

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            var query = context.Set<T>().AsQueryable();
            foreach (var item in includes)
            {
                query = query.Include(item);
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetAsync(int id)
       => await context.Set<T>().FindAsync(id);

        public async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> values = context.Set<T>().Where(x => x.Id == id);
            foreach (var item in includes)
            {
                values = values.Include(item);
            }
            return await values.FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(int id, T entity)
        {
            var entities = await context.Set<T>().FindAsync(id);
            context.Update(entities);
            await context.SaveChangesAsync();
        }
    }
}
