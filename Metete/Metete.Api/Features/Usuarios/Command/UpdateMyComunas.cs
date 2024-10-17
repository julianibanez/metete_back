using AutoMapper;
using MediatR;
using Metete.Api.Data;
using Metete.Api.Infraestructure.Exceptions;
using Metete.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Metete.Api.Features.Usuarios.Command
{
    public class UpdateMyComunas
    {
        public class Command : IRequest<Unit>
        {
            public List<int> Comunas { get; set; } = new ();
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
                    .Include(x => x.UsuarioComunas)
                    .FirstOrDefaultAsync(x => x.Id == contextUsuarioId, cancellationToken);

                if (usuario == null)
                {
                    throw new EntityNotFoundException(nameof(Usuario), contextUsuarioId);
                }

                // Comunas eliminadas
                var deletedComunas = usuario.UsuarioComunas.Where(x => !command.Comunas.Any(y => y == x.IdComuna));

                // Comunas nuevas
                var newComunas = command.Comunas.Where(x => !usuario.UsuarioComunas.Any(y => y.IdComuna == x))
                    .Select(x => new UsuarioComuna
                    {                        
                        IdComuna = x,
                    }).ToList();

                // Elimina las comunas quitadas
                _context.UsuarioComunas.RemoveRange(deletedComunas);

                // Agrega las comunas nuevas
                newComunas.ForEach(x => usuario.UsuarioComunas.Add(x));

                _context.Usuarios.Update(usuario);

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}