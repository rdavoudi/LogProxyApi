using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LPA.DataLayer.Context
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken());
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());

        int SaveChanges(bool acceptAllChangesOnSuccess);
        int SaveChanges();

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        
    }
}
