using MediatR;
using Metete.Api.Data;
using Metete.Api.Infraestructure.Exceptions;
using Metete.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Metete.Api.Features.Eventos.Commands
{
    public class MarkNotificacionAsRead
    {
        public class Command : IRequest<Unit>
        {
            public int Id { get; set; }          
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly MeteteContext _context;

            public Handler(MeteteContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
            {
                Notificacion? notificacion = await _context.Notificaciones.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

                if (notificacion == null)
                {
                    throw new EntityNotFoundException(nameof(Notificacion), command.Id);
                }

                notificacion.Leido = true;

                _context.Notificaciones.Update(notificacion);

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
