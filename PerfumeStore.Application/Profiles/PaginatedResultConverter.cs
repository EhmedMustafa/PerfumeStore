using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PerfumeStore.Domain.Entities;

namespace PerfumeStore.Application.Profiles
{
    public class PaginatedResultConverter<TSource, TDestination>
    : ITypeConverter<PaginatedResult<TSource>, PaginatedResult<TDestination>>
    {
        public PaginatedResult<TDestination> Convert(
            PaginatedResult<TSource> source,
            PaginatedResult<TDestination> destination,
            ResolutionContext context)
        {
            return new PaginatedResult<TDestination>
            {
                Items = context.Mapper.Map<List<TDestination>>(source.Items),
                TotalCount = source.TotalCount,
                PageSize = source.PageSize,
                CurrentPage = source.CurrentPage
            };
        }
    }
}
