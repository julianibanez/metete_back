using AutoMapper;
using MediatR;
using Metete.Api.Data;
using Metete.Api.Infraestructure.Exceptions;
using Metete.Api.Infraestructure.Extensions;
using Metete.Api.Models;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Metete.Api.Features.Eventos.Queries
{
    public class GetAllEventos
    {
        public class Query : IRequest<List<EventoDto>>
        {
            public string? TextoBusqueda { get; set; }
            public string? IdTipoDeportes { get; set; }
            public int? Km { get; set; }
            public double? Latitud { get; set; }
            public double? Longitud { get; set; }
            public DateTime? FechaDesde { get; set; }
            public DateTime? FechaHasta { get; set; }
            public TipoEvento? TipoEvento { get; set; }
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

                IQueryable<Evento> eventosQuery = _context.Eventos
                    .Include(x => x.CentroDeporte)
                        .ThenInclude(x => x.Comuna)
                    .Include(x => x.TipoDeporte)
                    .Include(x => x.Usuarios)
                    .AsNoTracking()
                    .Where(x => x.FechaEvento >= DateTime.UtcNow)
                    .OrderByDescending(x => x.FechaEvento);

                if (!string.IsNullOrWhiteSpace(query.TextoBusqueda))
                {
                    eventosQuery = eventosQuery.Where(x => x.Nombre.Contains(query.TextoBusqueda));
                }

                if (query.TipoEvento == TipoEvento.Preferencias)
                {
                    Usuario? usuario = await _context.Usuarios
                        .Include(x => x.UsuarioTipoDeportes)
                        .Include(x => x.UsuarioHorarios)
                        .Include(x => x.UsuarioComunas)
                            .ThenInclude(x => x.Comuna)
                        .FirstOrDefaultAsync(x => x.Id == contextUsuarioId, cancellationToken);

                    if (usuario == null)
                    {
                        throw new EntityNotFoundException(nameof(Usuario), contextUsuarioId);
                    }

                    if (usuario.UsuarioTipoDeportes.Count > 0)
                    {
                        List<int> ids = usuario.UsuarioTipoDeportes
                            .Select(x => x.IdTipoDeporte)
                            .ToList();

                        eventosQuery = eventosQuery.Where(x => ids.Contains(x.IdTipoDeporte));
                    }

                    if (usuario.UsuarioHorarios.Count > 0)
                    {
                        var firstHorario = usuario.UsuarioHorarios.First();

                        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(firstHorario.ZonaHoraria);
                        TimeSpan offset = timeZoneInfo.GetUtcOffset(DateTime.UtcNow);
                        string offsetFormatted = string.Format("{0:D2}:{1:D2}", offset.Hours, offset.Minutes);

                        Expression<Func<Evento, bool>> baseExpression = x => false;

                        foreach (var horario in usuario.UsuarioHorarios)
                        {
                            // Genera una expresón or por cada rango de horarios
                            baseExpression = baseExpression.Or(x =>
                                EF.Functions.ConvertTimeZone(x.FechaEvento, "+00:00", offsetFormatted)!.Value.DayOfWeek == (DayOfWeek)horario.DiaDeLaSemana
                                && EF.Functions.ConvertTimeZone(x.FechaEvento, "+00:00", offsetFormatted)!.Value.TimeOfDay >= horario.HorarioInicio
                                && EF.Functions.ConvertTimeZone(x.FechaEvento, "+00:00", offsetFormatted)!.Value.TimeOfDay <= horario.HorarioTermino);
                        }

                        eventosQuery = eventosQuery.Where(baseExpression);
                    }

                    if (usuario.UsuarioComunas.Count > 0)
                    {
                        List<string> names = usuario.UsuarioComunas
                            .Select(x => x.Comuna.Nombre.Trim())
                            .ToList();

                        Expression<Func<Evento, bool>> baseExpression = x => false;

                        foreach (var name in names)
                        {
                            baseExpression = baseExpression.Or(x => x.DireccionCentroDeporte != null && x.DireccionCentroDeporte.Contains(name));
                        }

                        eventosQuery = eventosQuery.Where(baseExpression);
                    }

                    //if (usuario.KmBusqueda > 0)
                    //{
                    //    int distanceInMetres = usuario.KmBusqueda.Value * 1000;
                    //    Point referencePoint = new(query.Longitud!.Value, query.Latitud!.Value) { SRID = 4326 };

                    //    eventosQuery = eventosQuery.Where(x =>
                    //        (!x.OtroCentroDeporte && x.CentroDeporte.Ubicacion.IsWithinDistance(referencePoint, distanceInMetres))
                    //        || (x.OtroCentroDeporte && x.UbicacionCentroDeporte!.IsWithinDistance(referencePoint, distanceInMetres)));
                    //}  
                }
                else if (query.TipoEvento == TipoEvento.MisEventos)
                {
                    eventosQuery = eventosQuery.Where(x => x.IdCreador == contextUsuarioId);
                }
                else if (query.TipoEvento == TipoEvento.Inscrito)
                {
                    eventosQuery = eventosQuery.Where(x => x.Usuarios.Any(y => y.IdUsuario == contextUsuarioId && (y.Aprobado ?? false)));
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(query.IdTipoDeportes))
                    {
                        List<int> ids = query.IdTipoDeportes.Split('|')
                            .Select(int.Parse)
                            .ToList();

                        eventosQuery = eventosQuery.Where(x => ids.Contains(x.IdTipoDeporte));
                    }

                    if (query.FechaDesde.HasValue)
                    {
                        eventosQuery = eventosQuery.Where(x => x.FechaEvento >= query.FechaDesde.Value);
                    }

                    if (query.FechaHasta.HasValue)
                    {
                        eventosQuery = eventosQuery.Where(x => x.FechaEvento <= query.FechaHasta.Value);
                    }

                    if (query.Km > 0)
                    {
                        int distanceInMetres = query.Km.Value * 1000;
                        Point referencePoint = new(query.Longitud!.Value, query.Latitud!.Value) { SRID = 4326 };

                        eventosQuery = eventosQuery.Where(x =>
                            (!x.OtroCentroDeporte && x.CentroDeporte.Ubicacion.IsWithinDistance(referencePoint, distanceInMetres))
                            || (x.OtroCentroDeporte && x.UbicacionCentroDeporte!.IsWithinDistance(referencePoint, distanceInMetres)));
                    }
                }

                List<Evento> eventos = await eventosQuery.ToListAsync(cancellationToken);

                List<EventoDto> result = [];

                foreach (Evento evento in eventos)
                {
                    EventoDto dto = _mapper.Map<EventoDto>(evento);

                    dto.Propio = evento.IdCreador == contextUsuarioId;
                    dto.Inscrito = evento.Usuarios.Any(x => x.IdUsuario == contextUsuarioId);
                    dto.Aprobado = evento.Usuarios.Any(x => x.IdUsuario == contextUsuarioId && (x.Aprobado ?? false));

                    result.Add(dto);
                }

                return result;
            }
        }

        public class EventoDto
        {
            public int Id { get; set; }
            public string Nombre { get; set; } = null!;
            public string NombreTipoDeporte { get; set; } = null!;
            public DateTime FechaEvento { get; set; }
            public int NumJugadores { get; set; }
            public string NombreCentroDeporte { get; set; } = null!;
            public string DireccionCentroDeporte { get; set; } = null!;
            public decimal Latitud { get; set; }
            public decimal Longitud { get; set; }
            public bool Propio { get; set; }
            public bool Inscrito { get; set; }
            public bool Aprobado { get; set; }
        }

        public enum TipoEvento
        {
            Preferencias = 1,
            MisEventos = 2,
            Inscrito = 3
        }
    }
}
