using System.Runtime.CompilerServices;
using Infra.Data.Interfaces.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;
using System.Threading.Tasks;
using Infra.Data.Context;

namespace Infra.Data.Impl.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        private DbContext Ctx { get; }
        private ITransaction CurrentTransaction { get; set; }

        public UnitOfWork(ApplicationDbContext ctx)
        {
            Ctx = ctx;
        }

        public Task<int> CommitAsync(CancellationToken ct)
        {
            return Ctx.SaveChangesAsync(ct);
        }

        public async Task<ITransaction> BeginTransactionAsync(CancellationToken ct)
        {
            if (CurrentTransaction == null)
            {
                var efTransaction = Ctx.Database.CurrentTransaction ?? await Ctx.Database.BeginTransactionAsync(ct);
                CurrentTransaction = new UnitOfWorkTransaction(efTransaction);
            }
            return CurrentTransaction;
        }

        private class UnitOfWorkTransaction : ITransaction
        {
            private bool _isCommited;
            public IDbContextTransaction DbContextTransaction { get; }
            public UnitOfWorkTransaction(IDbContextTransaction dbContextTransaction)
            {
                DbContextTransaction = dbContextTransaction;
            }

            public Task CommitAsync(CancellationToken ct)
            {
                DbContextTransaction.Commit();
                _isCommited = true;
                return Task.CompletedTask;
            }

            public void Dispose()
            {
                if (!_isCommited)
                {
                    DbContextTransaction.Rollback();
                }
            }
        }
    }
}
