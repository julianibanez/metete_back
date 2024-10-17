using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Metete.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Metete.Api.Features.TipoDeportes.Queries
{
    public class GetAllTipoDeportesForDropDownList
    {
        public class Query : IRequest<List<TipoDeporteDto>>
        {
        }

        public class Handler : IRequestHandler<Query, List<TipoDeporteDto>>
        {
            private readonly MeteteContext _context;
            private readonly IMapper _mapper;

            public Handler(MeteteContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<TipoDeporteDto>> Handle(Query query, CancellationToken cancellationToken)
            {
                List<TipoDeporteDto> tipoDeportes = await _context.TipoDeportes.AsNoTracking()
                     .OrderBy(x => x.Nombre)
                     .ProjectTo<TipoDeporteDto>(_mapper.ConfigurationProvider)
                     .ToListAsync();

                return tipoDeportes;
            }
        }

        public class TipoDeporteDto
        {
            public int Id { get; set; }

            public string Nombre { get; set; } = null!;
        }
    }
}
