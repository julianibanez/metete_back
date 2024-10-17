using AutoMapper;
using MediatR;
using Metete.Api.Data;
using Metete.Api.Infraestructure.Exceptions;
using Metete.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Metete.Api.Features.Usuarios.Command
{
    public class DeleteMyAccount
    {
        public class Command : IRequest<Unit>
        {
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

                Usuario? usuario = await _context.Usuarios
                    .Include(x => x.Notificaciones)
                    .Include(x => x.UsuarioEvaluaciones)
                    .Include(x => x.UsuarioEventos)                        
                    .Include(x => x.UsuarioHorarios)
                    .Include(x => x.UsuarioTipoDeportes)
                    .Include(x => x.UsuarioTipoNotificaciones)
                    .FirstOrDefaultAsync(x => x.Id == contextUsuarioId, cancellationToken);

                if (usuario == null)
                {
                    throw new EntityNotFoundException(nameof(Usuario), contextUsuarioId);
                }

                List<Evento> eventos = await _context.Eventos
                    .Include(x => x.Evaluaciones)
                    .Include(x => x.Notificaciones)
                    .Include(x => x.Usuarios)
                    .Where(x => x.IdCreador == contextUsuarioId)
                    .ToListAsync(cancellationToken);

                bool isOrganizer = _context.Eventos.Any(x => x.IdCreador == contextUsuarioId
                         && x.FechaEvento.Date >= DateTime.UtcNow.Date);

                if (isOrganizer)
                {
                    throw new BusinessRuleException("No puede eliminar su cuenta hasta que los eventos que ha organizado hayan finalizado");
                }

                _context.Notificaciones.RemoveRange(usuario.Notificaciones);
                _context.UsuarioEvaluaciones.RemoveRange(usuario.UsuarioEvaluaciones);
                _context.UsuarioEventos.RemoveRange(usuario.UsuarioEventos);
                _context.UsuarioHorarios.RemoveRange(usuario.UsuarioHorarios);
                _context.UsuarioTipoDeportes.RemoveRange(usuario.UsuarioTipoDeportes);
                _context.UsuarioTipoNotificaciones.RemoveRange(usuario.UsuarioTipoNotificaciones);

                _context.UsuarioEvaluaciones.RemoveRange(eventos.SelectMany(x => x.Evaluaciones));
                _context.Notificaciones.RemoveRange(eventos.SelectMany(x => x.Notificaciones));
                _context.UsuarioEventos.RemoveRange(eventos.SelectMany(x => x.Usuarios));
                _context.Eventos.RemoveRange(eventos);

                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}