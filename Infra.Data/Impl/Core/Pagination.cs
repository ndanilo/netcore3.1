using Infra.Data.Interfaces.Core;
using Microsoft.EntityFrameworkCore;
using Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Infra.Data.Impl.Core
{
    public class Pagination<TSource> : IPagination<TSource>
    {
        protected IQueryable<TSource> _query;

        protected virtual IQueryable<TSource> CountQuery => _query;
        protected virtual IQueryable<TSource> ListQuery => _query;

        public Pagination(IQueryable<TSource> query)
        {
            _query = query;
        }

        public IPagination<TSource> OrderBy<TKey>(Expression<Func<TSource, TKey>> keySelector) => new OrderedPagination<TSource, TKey>(_query, keySelector, false);
        public IPagination<TSource> OrderByDescending<TKey>(Expression<Func<TSource, TKey>> keySelector) => new OrderedPagination<TSource, TKey>(_query, keySelector, true);
        public async Task<PaginationResult<TSource>> GetPageAsync(int pageNumber, int pageSize, CancellationToken ct)
        {
            try
            {
                if (pageSize <= 0)
                {
                    throw new ArgumentException("pageSize can't be <= 0");
                }
                pageNumber = Math.Max(1, pageNumber);

                int count = await CountQuery.CountAsync(ct);
                IReadOnlyList<TSource> pageList = new TSource[0];

                if (count > 0)
                {
                    pageList = await ListQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(ct);
                }

                return new PaginationResult<TSource>(pageNumber, pageSize, count, pageList);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }

    internal class OrderedPagination<TSource, TKey> : Pagination<TSource>
    {
        protected Expression<Func<TSource, TKey>> _keySelector;
        protected bool _isDescending;

        protected override IQueryable<TSource> ListQuery => _isDescending ? _query.OrderByDescending(_keySelector) : _query.OrderBy(_keySelector);

        public OrderedPagination(IQueryable<TSource> query, Expression<Func<TSource, TKey>> keySelector, bool isDescending) : base(query)
        {
            _keySelector = keySelector;
            _isDescending = isDescending;
        }
    }
}
