using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonProject.PaginationService
{
    public static class Pagination
    {
        public static IEnumerable<TSource> ToPaged<TSource>(this IEnumerable<TSource> source , int Page , int PageCount)
        {
            return source.Skip(PageCount * (Page - 1)).Take(PageCount);
        }
    }
}
