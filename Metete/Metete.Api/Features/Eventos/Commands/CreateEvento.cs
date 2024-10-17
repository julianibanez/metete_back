using AutoMapper;
using MediatR;
using Metete.Api.Data;
using Metete.Api.Enums;
using Metete.Api.Models;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System.Security.Claims;

namespace Metete.Api.Features.Eventos.Commands
{
    public class CreateEvento
    {
        public class Command : IRequest<int>
        {
            public string Nombre { get; set; } = null!;

            public int IdTipoDeporte { get; set; }

            public string? Descripcion { get; set; }

            public DateTime FechaEvento { get; set; }

            public string ZonaHorariaEvento { get; set; } = null!;

            public int Duracion { get; set; }

            public int? IdCentroDeporte { get; set; }

            public bool OtroCentroDeporte { get; set; }

            public string? NombreCentroDeporte { get; set; }

            public string? DireccionCentroDeporte { get; set; }

            public decimal? LatitudCentroDeporte { get; set; }

            public decimal? LongitudCentroDeporte { get; set; }

            public int NumJugadores { get; set; }

            public decimal PrecioPorPersona { get; set; }

            public int IdMetodoPago { get; set; }

            public bool DevolucionAbandono { get; set; }

            public bool RecordarEventoJugador { get; set; }

            public bool ObligatorioDisponerTelefono { get; set; }

            public int IdCategoriaGeneroPermitido { get; set; }
        }

        public class Handler : IRequestHandler<Command, int>
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

            public async Task<int> Handle(Command command, CancellationToken cancellationToken)
            {
                int contextUsuarioId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                string contextUsername = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Email)!;

                Evento evento = _mapper.Map<Evento>(command);

                // TODO: Reemplazar por parámetro
                evento.IdTipoDivisa = 1;

                evento.IdCreador = contextUsuarioId;
                evento.Creador = contextUsername;
                evento.FechaCreacion = DateTime.UtcNow;
                evento.Modificador = contextUsername;
                evento.FechaModificacion = DateTime.UtcNow;

                if (command.OtroCentroDeporte)
                {
                    evento.UbicacionCentroDeporte = new Point((double)command.LongitudCentroDeporte!,
                        (double)command.LatitudCentroDeporte!)
                    { SRID = 4326 };
                }

                _context.Eventos.Add(evento);

                #region Notificación: NuevoEvento

                TipoNotificacion? tipoNotificacion = await _context.TipoNotificaciones.FirstOrDefaultAsync(x =>
                    x.Id == (int)TipoNotificacionEnum.NuevoEvento, cancellationToken);

                if (tipoNotificacion != null)
                {
                    // Crea un objeto DateTimeOffset con la fecha del evento UTC (zero offset)
                    var dateTimeOffset = new DateTimeOffset(command.FechaEvento);

                    // Define la zona horaria del evento
                    var targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById(command.ZonaHorariaEvento);

                    // Convierte el DateTimeOffset a la zona horaria del evento
                    var targetDateTime = TimeZoneInfo.ConvertTime(dateTimeOffset, targetTimeZone);

                    // Obtiene todos los usuarios donde el evento está dentro de sus preferencias + los usuarios que no tienen preferencias
                    var usuarios = await _context.Usuarios
                        .Include(x => x.UsuarioTipoDeportes)
                        .Include(x => x.UsuarioHorarios)
                        .Include(x => x.UsuarioComunas)
                            .ThenInclude(x => x.Comuna)
                        .Where(x =>
                            (x.UsuarioTipoDeportes.Count() == 0 || x.UsuarioTipoDeportes.Any(y => y.IdTipoDeporte == command.IdTipoDeporte))
                            && (x.UsuarioHorarios.Count() == 0 || (x.UsuarioHorarios.Any(y => y.DiaDeLaSemana == (int)targetDateTime.DayOfWeek
                                && targetDateTime.TimeOfDay >= y.HorarioInicio
                                && targetDateTime.TimeOfDay <= y.HorarioTermino)))
                            && (x.UsuarioComunas.Count() == 0 || x.UsuarioComunas.Any(y => command.DireccionCentroDeporte!.Contains(y.Comuna.Nombre.Trim()))))
                        .ToListAsync(cancellationToken);

                    var ids = usuarios.Where(x => x.Id != contextUsuarioId).Select(x => x.Id).Distinct().ToList();

                    foreach (int id in ids)
                    {
                        evento.Notificaciones.Add(new Notificacion
                        {
                            IdUsuario = id,
                            IdTipoNotificacion = tipoNotificacion.Id,
                            Titulo = tipoNotificacion.Titulo,
                            Mensaje = tipoNotificacion.Mensaje,
                            IdEstadoNotificacion = (int)EstadoNotificacion.Pendiente,
                            FechaCreacion = DateTime.UtcNow
                        });
                    };
                }
                #endregion

                await _context.SaveChangesAsync(cancellationToken);

                return evento.Id;
            }
        }
    }
}
