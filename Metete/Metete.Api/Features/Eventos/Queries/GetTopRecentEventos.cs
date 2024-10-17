using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Metete.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Metete.Api.Features.Eventos.Queries
{
    public class GetTopRecentEventos
    {
        public class Query : IRequest<List<EventoDto>>
        {
        }

        public class Handler : IRequestHandler<Query, List<EventoDto>>
        {
            private readonly MeteteContext _context;
            private readonly IMapper _mapper;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(MeteteContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            {
                _context = context;
                _mapper = mapper;
                _httpContextAccessor = httpContextAccessor; 
            }

            public async Task<List<EventoDto>> Handle(Query query, CancellationToken cancellationToken)
            {
                int contextUsuarioId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                List<EventoDto> eventos = await _context.Eventos
                    .Include(x => x.CentroDeporte)
                        .ThenInclude(x => x.Comuna)
                    .AsNoTracking()                    
                    .OrderByDescending(x => x.FechaEvento)
                    .Take(5)
                    .ProjectTo<EventoDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                eventos.ForEach(x => x.Propio = x.IdCreador == contextUsuarioId);

                return eventos;
            }
        }

        public class EventoDto
        {
            public int Id { get; set; }
            public string Nombre { get; set; } = null!;
            public DateTime FechaEvento { get; set; }
            public int NumJugadores { get; set; }
            public string NombreCentroDeporte { get; set; } = null!;
            public string DireccionCentroDeporte { get; set; } = null!;
            public decimal Latitud { get; set; }
            public decimal Longitud { get; set; }
            public int IdCreador { get; set; }
            public bool Propio { get; set; }
        }
    }
}
