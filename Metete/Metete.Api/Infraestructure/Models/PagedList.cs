using Microsoft.EntityFrameworkCore;

namespace Metete.Api.Infraestructure.Models
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; }

        public int PageNumber { get; }

        public int TotalPages { get; }

        public int TotalCount { get; }

        public PaginatedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            Items = items;
        }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, PaginationData paginationData)
        {
            int count = await source.CountAsync();
            List<T> items = await source.Skip((paginationData.PageNumber - 1) * paginationData.PageSize).Take(paginationData.PageSize).ToListAsync();

            return new PaginatedList<T>(items, count, paginationData.PageNumber, paginationData.PageSize);
        }
    }
}
