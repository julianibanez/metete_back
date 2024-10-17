using AutoMapper.QueryableExtensions;
using Metete.Api.Infraestructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Metete.Api.Infraestructure.Extensions
{
    public static class PaginationExtesions
    {
        public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, PaginationData paginationData)
             => PaginatedList<TDestination>.CreateAsync(queryable, paginationData);

        public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, AutoMapper.IConfigurationProvider configuration)
            => queryable.ProjectTo<TDestination>(configuration).ToListAsync();
    }
}
