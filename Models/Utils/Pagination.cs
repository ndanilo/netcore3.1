using System;
using System.Collections.Generic;

namespace Models.Utils
{
    public class PaginationResult<T>
    {
        public int PageNumber { get; }
        public int PageSize { get; }
        public int TotalCount { get; }
        public IReadOnlyList<T> PageList { get; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        public PaginationResult(int pageNumber, int pageSize, int totalCount, IReadOnlyList<T> pageList)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
            PageList = pageList;
        }
    }
}
