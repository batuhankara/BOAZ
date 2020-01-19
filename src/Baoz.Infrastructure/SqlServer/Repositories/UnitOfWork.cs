using System;
using System.Linq;
using System.Threading.Tasks;
using Baoz.Infrastructure.SqlServer.Contracts;
using EventFlow.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace Baoz.Infrastructure.SqlServer.Repositories
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : IDbContext
    {
        private bool disposed = false;
        private readonly TContext _context;

        public UnitOfWork(TContext context)
        {
            _context = context;
        }

        public void ChangeAutoDetectChangesStatus(bool parameter = true)
        {
            (_context as DbContext).ChangeTracker.AutoDetectChangesEnabled = parameter;
        }

        public int Commit()
        {
            return SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await SaveChangesAsync();
        }

        private int SaveChanges()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.First();
                //var entity = JsonConvert.SerializeObject(entry.Entity);
                throw new OptimisticConcurrencyException($"Failed to save {entry.Entity} because it was changed in the database", ex);
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw new OptimisticConcurrencyException(ex.Message);
            }

           
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }

        private async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                EntityEntry entry = ex.Entries.First();

                string entity = JsonConvert.SerializeObject(entry.Entity);

                throw new OptimisticConcurrencyException($"Failed to save {entry.Entity} because it was changed in the database", ex);
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw new OptimisticConcurrencyException(ex.Message);
            }
            //catch (DbEntityValidationException ex)
            //{
            //    throw new InvalidOperationException(BeautifyDatabaseExceptionMessage(ex), ex);
            //}
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }

       
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            this.disposed = true;
        }

        public void DetachAll()
        {
            foreach (EntityEntry dbEntityEntry in (_context as DbContext).ChangeTracker.Entries())
            {
                if (dbEntityEntry.Entity != null)
                {
                    dbEntityEntry.State = EntityState.Detached;
                }
            }
        }
    }

}
