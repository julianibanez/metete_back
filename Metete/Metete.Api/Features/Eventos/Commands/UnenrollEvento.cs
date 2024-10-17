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
    public class UnenrollEvento
    {
        public class Command : IRequest<Unit>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly MeteteContext _context;
            private readonly IMapper _mapper;
            private readonly IConfiguration _configuration;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(MeteteContext context, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            {
                _context = context;
                _mapper = mapper;
                _configuration = configuration;
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

                UsuarioEvento? usuarioEvento = evento.Usuarios.FirstOrDefault(x => x.IdUsuario == contextUsuarioId);

                if (usuarioEvento == null)
                {
                    throw new EntityNotFoundException(nameof(UsuarioEvento), command.Id);
                }
                
                // Elimina usuario del evento
                evento.Usuarios.Remove(usuarioEvento);
                _context.UsuarioEventos.Remove(usuarioEvento);

                // Notifica al organizador
                TipoNotificacion? tipoNotificacion = await _context.TipoNotificaciones.FirstOrDefaultAsync(x =>
                    x.Id == (int)TipoNotificacionEnum.JugadorAbandonaEvento);

                if (tipoNotificacion != null)
                {
                    evento.Notificaciones.Add(new Notificacion
                    {
                        IdUsuario = evento.IdCreador,
                        IdTipoNotificacion = tipoNotificacion.Id,
                        Titulo = tipoNotificacion.Titulo,
                        Mensaje = tipoNotificacion.Mensaje,
                        IdEvento = evento.Id,
                        IdEstadoNotificacion = (int)EstadoNotificacion.Pendiente,
                        FechaCreacion = DateTime.UtcNow
                    });
                }

                _context.Eventos.Update(evento);

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
