using AutoMapper;
using MediatR;
using Metete.Api.Data;
using Metete.Api.Enums;
using Metete.Api.Infraestructure.Exceptions;
using Metete.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Metete.Api.Features.Eventos.Commands
{
    public class CreateChatNotificacion
    {
        public class Command : IRequest<Unit>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Unit>
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

            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
            {
                int contextUsuarioId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                Evento? evento = await _context.Eventos
                    .Include(x => x.Usuarios)
                    .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

                if (evento == null)
                {
                    throw new EntityNotFoundException(nameof(Evento), command.Id);
                }
                
                // Notifica a los usuarios aprobados del evento
                TipoNotificacion? tipoNotificacion = await _context.TipoNotificaciones.FirstOrDefaultAsync(x =>
                    x.Id == (int)TipoNotificacionEnum.NuevoMesajeChatEvento);

                if (tipoNotificacion != null)
                {
                    var usuarios = evento.Usuarios
                        .Where(x => (x.Aprobado ?? false) && x.IdUsuario != contextUsuarioId)
                        .Select(x => x.IdUsuario)
                        .ToList();

                    // Agrega organizador
                    if (!usuarios.Contains(evento.IdCreador) && evento.IdCreador != contextUsuarioId) {
                        usuarios.Add(evento.IdCreador);
                    }

                    foreach (var idUsuario in usuarios)
                    {
                        Notificacion notificacion = new Notificacion
                        {
                            IdUsuario = idUsuario,
                            IdTipoNotificacion = (int)TipoNotificacionEnum.NuevoMesajeChatEvento,
                            Titulo = tipoNotificacion.Titulo,
                            Mensaje = tipoNotificacion.Mensaje.Replace("{NOMBRE_EVENTO}", evento.Nombre),
                            IdEvento = command.Id,
                            IdEstadoNotificacion = (int)EstadoNotificacion.Pendiente,
                            FechaCreacion = DateTime.UtcNow,
                        };

                        _context.Notificaciones.Add(notificacion);
                    }

                    await _context.SaveChangesAsync(cancellationToken);
                }

                return Unit.Value;
            }
        }
    }
}
