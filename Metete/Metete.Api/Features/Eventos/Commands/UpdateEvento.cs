using AutoMapper;
using MediatR;
using Metete.Api.Data;
using Metete.Api.Enums;
using Metete.Api.Features.Eventos.Queries;
using Metete.Api.Infraestructure.Exceptions;
using Metete.Api.Models;
using Microsoft.EntityFrameworkCore;


namespace Metete.Api.Features.Eventos.Commands
{
    public class UpdateEvento
    {
        public class Command : IRequest<Unit>
        {
            public int Id { get; set; }

            public string Nombre { get; set; } = null!;

            public string? Descripcion { get; set; }

            public DateTime FechaEvento { get; set; }

            public int Duracion { get; set; }

            public int? IdCentroDeporte { get; set; }

            public int NumJugadores { get; set; }

            public decimal PrecioPorPersona { get; set; }

            public int IdMetodoPago { get; set; }           

            public bool DevolucionAbandono { get; set; }

            public bool RecordarEventoJugador { get; set; }

            public bool ObligatorioDisponerTelefono { get; set; }

            public int IdCategoriaGeneroPermitido { get; set; }
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
                Evento? evento = await _context.Eventos.FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

                if (evento == null)
                {
                    throw new EntityNotFoundException(nameof(Evento), command.Id);
                }


                // Obtener participantes desde la base de datos directamente
                var participantes = await _context.UsuarioEventos
                    .Where(p => p.IdEvento == command.Id)
                    .ToListAsync(cancellationToken);


                // Notifica al organizador
                TipoNotificacion? tipoNotificacion = await _context.TipoNotificaciones.FirstOrDefaultAsync(x =>
                    x.Id == (int)TipoNotificacionEnum.EventoModificado);

                if (tipoNotificacion != null)
                {
                    for (int i = 0; i < participantes.Count; i++)
                    {
                        evento.Notificaciones.Add(new Notificacion
                        {
                            IdUsuario = participantes[i].IdUsuario,
                            IdTipoNotificacion = tipoNotificacion.Id,
                            Titulo = tipoNotificacion.Titulo,
                            Mensaje = tipoNotificacion.Mensaje,
                            IdEvento = evento.Id,
                            IdEstadoNotificacion = (int)EstadoNotificacion.Pendiente,
                            FechaCreacion = DateTime.UtcNow
                        });
                    }
                }


                _mapper.Map(command, evento);

                _context.Eventos.Update(evento);

                await _context.SaveChangesAsync(cancellationToken);

             


                return Unit.Value;
            }
        }
    }
}
