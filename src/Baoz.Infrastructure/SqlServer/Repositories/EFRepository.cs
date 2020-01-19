using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Baoz.Infrastructure.SqlServer.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Baoz.Infrastructure.SqlServer.Repositories
{
    public class EFRepository<TEntity> : IEFRepository<TEntity> where TEntity : class
    {
        protected readonly DbSet<TEntity> DbSet;
        protected readonly IDbContext CurrentContext;

        public EFRepository(IDbContext dbContext)
        {
            CurrentContext = dbContext;
            DbSet = dbContext.Set<TEntity>();
        }
        public Task Add(TEntity entity)
        {
            DbSet.Add(entity);
            return Task.FromResult(0);
        }
        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filterBy)
        {
            return await DbSet.FirstOrDefaultAsync(filterBy);
        }



        public async Task<List<TEntity>> GetManyAsNoTrackingAsync(Expression<Func<TEntity, bool>> filterBy = null)
        {
            return (await DbSet.Where(filterBy).AsNoTracking().ToListAsync());
        }

    }

}
