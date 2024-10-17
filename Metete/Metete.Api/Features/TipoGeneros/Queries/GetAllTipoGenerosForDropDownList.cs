using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Metete.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Metete.Api.Features.TipoGeneros.Queries
{
    public class GetAllTipoGenerosForDropDownList
    {
        public class Query : IRequest<List<TipoGeneroDto>>
        {
        }

        public class Handler : IRequestHandler<Query, List<TipoGeneroDto>>
        {
            private readonly MeteteContext _context;
            private readonly IMapper _mapper;

            public Handler(MeteteContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<TipoGeneroDto>> Handle(Query query, CancellationToken cancellationToken)
            {
                List<TipoGeneroDto> tipoGeneros = await _context.TipoGeneros.AsNoTracking()
                     .OrderBy(x => x.Nombre)
                     .ProjectTo<TipoGeneroDto>(_mapper.ConfigurationProvider)
                     .ToListAsync();

                return tipoGeneros;
            }
        }

        public class TipoGeneroDto
        {
            public int Id { get; set; }

            public string Nombre { get; set; } = null!;
        }
    }
}
