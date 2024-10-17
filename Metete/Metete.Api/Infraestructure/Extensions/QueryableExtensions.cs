using Metete.Api.Infraestructure.Models;
using System.Linq.Dynamic.Core;

namespace Metete.Api.Infraestructure.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, PaginationData paginationData)
        {
            if (string.IsNullOrWhiteSpace(paginationData.SortField))
            {
                return source;
            }

            string? sortOrder = (paginationData.SortOrder ?? "").ToLower() switch
            {
                "ascend" => "asc",
                "descend" => "desc",
                _ => paginationData.SortOrder
            };

            source = source.OrderBy($"{paginationData.SortField} {sortOrder}");

            return source;
        }
    }
}
