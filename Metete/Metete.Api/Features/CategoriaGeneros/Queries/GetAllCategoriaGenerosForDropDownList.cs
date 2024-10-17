using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Metete.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Metete.Api.Features.CategoriaGeneros.Queries
{
    public class GetAllCategoriaGenerosForDropDownList
    {
        public class Query : IRequest<List<CategoriaGeneroDto>>
        {
        }

        public class Handler : IRequestHandler<Query, List<CategoriaGeneroDto>>
        {
            private readonly MeteteContext _context;
            private readonly IMapper _mapper;

            public Handler(MeteteContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<CategoriaGeneroDto>> Handle(Query query, CancellationToken cancellationToken)
            {
                List<CategoriaGeneroDto> categoriaGeneros = await _context.CategoriaGeneros.AsNoTracking()
                     .OrderBy(x => x.Nombre)
                     .ProjectTo<CategoriaGeneroDto>(_mapper.ConfigurationProvider)
                     .ToListAsync();

                return categoriaGeneros;
            }
        }

        public class CategoriaGeneroDto
        {
            public int Id { get; set; }

            public string Nombre { get; set; } = null!;
        }
    }
}
