using Microsoft.EntityFrameworkCore;
using Infra.Data.Interfaces.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Infra.Data.Context;
using Models.Interfaces.Core;

namespace Infra.Data.Impl.Core
{
    
    public abstract class Repository<TEntity> : IDisposable, IRepository<TEntity>
        where TEntity : class, IEntity
    {
        protected DbSet<TEntity> DbSet;
        protected IQueryable<TEntity> Query => DbSet.AsNoTracking();

        public Type ElementType => Query.ElementType;

        public Expression Expression => Query.Expression;

        public IQueryProvider Provider => Query.Provider;

        protected ApplicationDbContext Ctx { get; }

        protected Repository(ApplicationDbContext ctx)
        {
            Ctx = ctx;
            DbSet = ctx.Set<TEntity>();
        }

        public virtual void Add(TEntity obj)
        {
            DbSet.Add(obj);
        }

        public virtual void Update(TEntity obj)
        {
            DbSet.Attach(obj);
            Ctx.Entry(obj).State = EntityState.Modified;
        }

        public virtual void Delete(TEntity obj)
        {
            DbSet.Remove(obj);
        }

        public void Dispose()
        {
            Ctx.Dispose();
        }

        public IEnumerator<TEntity> GetEnumerator() => Query.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => (Query as IEnumerable).GetEnumerator();

        public IQueryable<TEntity> Tracking => DbSet;
    }
}
