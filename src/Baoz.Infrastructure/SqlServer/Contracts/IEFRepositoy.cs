using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Baoz.Infrastructure.SqlServer.Contracts
{
    public interface IEFRepository<TEntity> where TEntity : class
    {
        TEntity Add(TEntity entity);
        bool Any(Expression<Func<TEntity, bool>> filterBy = null);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filterBy = null);
        int Count(Expression<Func<TEntity, bool>> filterBy = null);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> filterBy = null);
        void Delete(object id);
        void Delete(TEntity deletedEntity);
        TEntity Get(object id);
        TEntity Get(Expression<Func<TEntity, bool>> filterBy);
        IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> filterBy = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filterBy);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filterBy = null, params string[] include);
        List<TEntity> GetMany(Expression<Func<TEntity, bool>> filterBy = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        Task<List<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> filterBy = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        Task<List<TEntity>> GetManyAsync(Expression<Func<TEntity, bool>> filterBy = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params string[] include);
        List<TEntity> GetManyAsNoTracking(Expression<Func<TEntity, bool>> filterBy = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        List<TEntity> GetManyAsNoTracking(Expression<Func<TEntity, bool>> filterBy = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params string[] include);
        Task<List<TEntity>> GetManyAsNoTrackingAsync(Expression<Func<TEntity, bool>> filterBy = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        Task<List<TEntity>> GetManyAsNoTrackingAsync(Expression<Func<TEntity, bool>> filterBy = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params string[] nclude);
  
      
    }
}
