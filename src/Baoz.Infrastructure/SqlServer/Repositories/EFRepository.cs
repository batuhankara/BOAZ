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
        //public string TableName => CurrentContext.GetTableName<TEntity>();
     
        public TEntity Add(TEntity entity)
        {
            return DbSet.Add(entity).Entity;
        }

        public bool Any(Expression<Func<TEntity, bool>> filterBy = null)
        {
            return GetQueryable(filterBy).Any();
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filterBy = null)
        {
            return await GetQueryable(filterBy).AnyAsync();
        }

        public int Count(Expression<Func<TEntity, bool>> filterBy = null)
        {
            return GetQueryable(filterBy).Count();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> filterBy = null)
        {
            return await GetQueryable(filterBy).CountAsync();
        }

        public void Delete(TEntity deletedEntity)
        {
            if (CurrentContext.Entry(deletedEntity).State == EntityState.Detached)
            {
                DbSet.Attach(deletedEntity);
            }

            DbSet.Remove(deletedEntity);
        }

        public void Delete(object id)
        {
            TEntity deletedEntity = DbSet.Find(id);

            Delete(deletedEntity);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filterBy)
        {
            return GetQueryable(filterBy).FirstOrDefault();
        }

        public TEntity Get(object id)
        {
            return DbSet.Find(id);
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filterBy)
        {
            return await GetQueryable(filterBy).FirstOrDefaultAsync();
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filterBy = null, params string[] include)
        {
            IQueryable<TEntity> query = GetQueryable(filterBy);

            if (include != null && include.Count() > 0)
            {
                foreach (var i in include)
                {
                    query = query.Include(i);
                }
            }

            return await query.FirstOrDefaultAsync();

        }

        public List<TEntity> GetMany(Expression<Func<TEntity, bool>> filterBy = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            return GetQueryable(filterBy, orderBy).ToList();
        }

        public List<TEntity> GetManyAsNoTracking(Expression<Func<TEntity, bool>> filterBy = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            return GetQueryable(filterBy, orderBy).AsNoTracking().ToList();
        }

        public List<TEntity> GetManyAsNoTracking(Expression<Func<TEntity, bool>> filterBy = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params string[] include)
        {
            IQueryable<TEntity> query = GetQueryable(filterBy, orderBy).AsNoTracking();

            if (include != null && include.Any())
            {
                foreach (string i in include)
                {
                    query = query.Include(i);
                }
            }

            return query.ToList();
        }

        public async Task<List<TEntity>> GetManyAsNoTrackingAsync(Expression<Func<TEntity, bool>> filterBy = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            return await GetQueryable(filterBy, orderBy).AsNoTracking().ToListAsync();
        }

        public async Task<List<TEntity>> GetManyAsNoTrackingAsync(Expression<Func<TEntity, bool>> filterBy = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params string[] include)
        {
            IQueryable<TEntity> query = GetQueryable(filterBy, orderBy).AsNoTracking();

            if (include != null && include.Any())
            {
                query = include.Aggregate(query, (current, i) => current.Include(i));
            }

            return await query.ToListAsync();
        }

        public async Task<List<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> filterBy = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            return await GetQueryable(filterBy, orderBy).ToListAsync();
        }

        public async Task<List<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> filterBy = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params string[] include)
        {
            IQueryable<TEntity> query = GetQueryable(filterBy, orderBy);

            if (include != null && include.Any())
            {
                foreach (string i in include)
                {
                    query = query.Include(i);
                }
            }

            return await query.ToListAsync();

        }

        public IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> filterBy = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = DbSet.AsQueryable();

            if (filterBy != null)
            {
                query = query.Where(filterBy);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query;
        }


        public TEntity Get<TId>(TId viewId)
        {
            return this.Get((object)viewId);
        }

    }

}
