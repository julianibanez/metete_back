using AutoMapper;
using MediatR;
using Metete.Api.Data;
using Metete.Api.Infraestructure.Exceptions;
using Metete.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Metete.Api.Features.Eventos.Commands
{
    public class DeleteEvento
    {
        public class Command : IRequest<Unit>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly MeteteContext _context;
            private readonly IMapper _mapper;

            public Handler(MeteteContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
            {
                Evento? evento = await _context.Eventos
                    .Include(x => x.Usuarios)
                    .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

                if (evento == null)
                {
                    throw new EntityNotFoundException(nameof(Evento), command.Id);
                }

                // TODO: Validar si el evento es propio antes de eliminar

                _context.UsuarioEventos.RemoveRange(evento.Usuarios);
                _context.Eventos.Remove(evento);

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
