using AutoMapper;
using MediatR;
using Metete.Api.Data;
using Metete.Api.Infraestructure.Exceptions;
using Metete.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Metete.Api.Features.Usuarios.Command
{
    public class UpdateMyDeportes
    {
        public class Command : IRequest<Unit>
        {
            public List<int> TipoDeportes { get; set; } = new ();
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly MeteteContext _context;
            private readonly IMapper _mapper;
            private readonly IConfiguration _configuration;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(MeteteContext context, IMapper mapper, IConfiguration configuration,  IHttpContextAccessor httpContextAccessor)
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
                    .Include(x => x.UsuarioTipoDeportes)
                    .FirstOrDefaultAsync(x => x.Id == contextUsuarioId, cancellationToken);

                if (usuario == null)
                {
                    throw new EntityNotFoundException(nameof(Usuario), contextUsuarioId);
                }

                // Deportes eliminados
                var deletedTipoDeportes = usuario.UsuarioTipoDeportes.Where(x => !command.TipoDeportes.Any(y => y == x.IdTipoDeporte));

                // Deportes nuevos
                var newTipoDeportes = command.TipoDeportes.Where(x => !usuario.UsuarioTipoDeportes.Any(y => y.IdTipoDeporte == x))
                    .Select(x => new UsuarioTipoDeporte
                    {                        
                        IdTipoDeporte = x,
                    }).ToList();

                // Elimina los deportes quitados
                _context.UsuarioTipoDeportes.RemoveRange(deletedTipoDeportes);

                // Agrega los deportes nuevos
                newTipoDeportes.ForEach(x => usuario.UsuarioTipoDeportes.Add(x));

                _context.Usuarios.Update(usuario);

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}