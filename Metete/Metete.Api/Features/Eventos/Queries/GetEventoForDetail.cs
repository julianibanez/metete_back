using AutoMapper;
using MediatR;
using Metete.Api.Data;
using Metete.Api.Infraestructure.Exceptions;
using Metete.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Metete.Api.Features.Eventos.Queries
{
    public class GetEventoForDetail
    {
        public class Query : IRequest<EventoDto>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, EventoDto>
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

            public async Task<EventoDto> Handle(Query query, CancellationToken cancellationToken)
            {
                int contextUsuarioId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                Evento? evento = await _context.Eventos
                    .Include(x => x.CentroDeporte)
                        .ThenInclude(x => x.Comuna)
                        .ThenInclude(x => x.Region)
                    .Include(x => x.CategoriaGenero)
                    .Include(x => x.MetodoPago)
                    .Include(x => x.UsuarioCreador)
                    .Include(x => x.Usuarios)
                    .Include(x => x.TipoDeporte)
                    .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

                if (evento == null)
                {
                    throw new EntityNotFoundException(nameof(Evento), query.Id);
                }

                if (evento.FechaEvento < DateTime.UtcNow)
                {
                    throw new BusinessRuleException("Lo sentimos, el evento ha terminado. Tendremos más eventos en el futuro, mantente actualizado");
                }

                // Determina si el creador del evento es el usuario en contexto
                bool isOwnEvento = evento.IdCreador == contextUsuarioId;

                EventoDto result = new()
                {
                    Id = query.Id,
                    NombreEvento = evento.Nombre,
                    NombreTipoDeporte = evento.TipoDeporte.Nombre,
                    NombreCentroDeporte = evento.OtroCentroDeporte ? evento.NombreCentroDeporte! : evento.CentroDeporte.Nombre,
                    DireccionCentroDeporte = evento.OtroCentroDeporte ? evento.DireccionCentroDeporte! : string.Join(", ", evento.CentroDeporte.Direccion, evento.CentroDeporte.Comuna.Nombre),
                    FechaEvento = DateTime.SpecifyKind(evento.FechaEvento, DateTimeKind.Utc),
                    NombreCategoriaGenero = evento.CategoriaGenero.Nombre,
                    NumJugadores = evento.NumJugadores,
                    Duracion = evento.Duracion,
                    PrecioPorPersona = evento.PrecioPorPersona,
                    NombreMetodoPago = evento.MetodoPago.Nombre,
                    NombreCreador = evento.UsuarioCreador.Nombre,
                    Latitud = evento.OtroCentroDeporte ? evento.LatitudCentroDeporte!.Value : evento.CentroDeporte.Latitud,
                    Longitud = evento.OtroCentroDeporte ? evento.LongitudCentroDeporte!.Value : evento.CentroDeporte.Longitud,
                    Propio = isOwnEvento,
                    Aprobado = evento.Usuarios.Any(x => x.IdUsuario == contextUsuarioId && (x.Aprobado ?? false)),
                    Inscrito = evento.Usuarios.Any(x => x.IdUsuario == contextUsuarioId),
                    RequierePosicion = evento.TipoDeporte.RequierePosicion
                };

                return result;
            }
        }

        public class EventoDto
        {
            public int Id { get; set; }
            public string NombreEvento { get; set; } = null!;
            public string NombreTipoDeporte { get; set; } = null!;
            public string NombreCentroDeporte { get; set; } = null!;
            public string DireccionCentroDeporte { get; set; } = null!;
            public DateTime FechaEvento { get; set; }
            public string NombreCategoriaGenero { get; set; } = null!;
            public int NumJugadores { get; set; }
            public int Duracion { get; set; }
            public decimal PrecioPorPersona { get; set; }
            public string NombreMetodoPago { get; set; } = null!;
            public string NombreCreador { get; set; } = null!;
            public decimal Latitud { get; set; }
            public decimal Longitud { get; set; }
            public bool Propio { get; set; }
            public bool Inscrito { get; set; }
            public bool Aprobado { get; set; }
            public bool RequierePosicion { get; set; }
        }
    }
}
