using AutoMapper;
using MediatR;
using Metete.Api.Data;
using Metete.Api.Enums;
using Metete.Api.Infraestructure.Exceptions;
using Metete.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Metete.Api.Features.Eventos.Commands
{
    public class RejectEnrollment
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

            public Handler(MeteteContext context, IMapper mapper, IConfiguration configuration)
            {
                _context = context;
                _mapper = mapper;
                _configuration = configuration;
            }

            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
            {
                UsuarioEvento? usuarioEvento = await _context.UsuarioEventos                    
                    .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

                if (usuarioEvento == null)
                {
                    throw new EntityNotFoundException(nameof(UsuarioEvento), command.Id);
                }

                usuarioEvento.Aprobado = false;
                
                _context.UsuarioEventos.Update(usuarioEvento);

                // Notifica al solicitante
                TipoNotificacion? tipoNotificacion = await _context.TipoNotificaciones.FirstOrDefaultAsync(x =>
                    x.Id == (int)TipoNotificacionEnum.RechazoEvento);

                if (tipoNotificacion != null)
                {
                    Notificacion notificacion = new()
                    {
                        IdUsuario = usuarioEvento.IdUsuario,
                        IdEvento = usuarioEvento.IdEvento,
                        IdTipoNotificacion = tipoNotificacion.Id,
                        Titulo = tipoNotificacion.Titulo,
                        Mensaje = tipoNotificacion.Mensaje,
                        IdEstadoNotificacion = (int)EstadoNotificacion.Pendiente,
                        FechaCreacion = DateTime.UtcNow
                    };

                    _context.Notificaciones.Add(notificacion);
                }

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
