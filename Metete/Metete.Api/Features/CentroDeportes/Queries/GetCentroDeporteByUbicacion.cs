using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Metete.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Metete.Api.Features.CentroDeportes.Queries
{
    public class GetCentroDeporteByUbicacion
    {
        public class Query : IRequest<List<CentroDeporteDto>>
        {
            public string Ubicacion { get; set; } = null!;
        }

        public class Handler : IRequestHandler<Query, List<CentroDeporteDto>>
        {
            private readonly MeteteContext _context;
            private readonly IMapper _mapper;

            public Handler(MeteteContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<CentroDeporteDto>> Handle(Query query, CancellationToken cancellationToken)
            {
                List<CentroDeporteDto> centroDeportes = await _context.CentroDeportes
                    .Include(x => x.Comuna)
                        .ThenInclude(x => x.Region)
                    .AsNoTracking()
                    .OrderBy(x => x.Nombre)
                    .Where(x => x.Direccion.Contains(query.Ubicacion)
                        || x.Comuna.Nombre.Contains(query.Ubicacion)
                        || x.Comuna.Region.Nombre.Contains(query.Ubicacion))                    
                    .ProjectTo<CentroDeporteDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return centroDeportes;
            }
        }

        public class CentroDeporteDto
        {
            public int Id { get; set; }
            public string Nombre { get; set; } = null!;
            public string Direccion { get; set; } = null!;
            public decimal Latitud { get; set; }
            public decimal Longitud { get; set; }
        }
    }
}
