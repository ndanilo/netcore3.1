using Models.Utils;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Infra.Data.Interfaces.Core
{
    public interface IPagination<TSource>
    {
        IPagination<TSource> OrderBy<TKey>(Expression<Func<TSource, TKey>> keySelector);
        IPagination<TSource> OrderByDescending<TKey>(Expression<Func<TSource, TKey>> keySelector);
        Task<PaginationResult<TSource>> GetPageAsync(int pageNumber, int pageSize, CancellationToken ct);
    }
}
