using Infra.Data.Impl.Core;
using Infra.Data.Interfaces.Core;
using System.Linq;

namespace Infra.Data.Extensions
{
    public static class QueryExtensions
    {
        public static IPagination<T> Paginate<T>(this IQueryable<T> query) => new Pagination<T>(query);
    }
}
