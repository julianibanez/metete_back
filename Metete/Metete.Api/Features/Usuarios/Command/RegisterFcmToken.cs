using MediatR;
using Metete.Api.Data;
using Metete.Api.Infraestructure.Exceptions;
using Metete.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Metete.Api.Features.Usuarios.Command
{
    public class RegisterFcmToken
    {
        public class Command : IRequest<Unit>
        {
            public string Token { get; set; } = null!;
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly MeteteContext _context;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(MeteteContext context, IHttpContextAccessor httpContextAccessor)
            {
                _context = context;
                _httpContextAccessor = httpContextAccessor; 
            }

            public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
            {
                int contextUsuarioId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                Usuario? usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == contextUsuarioId, cancellationToken);

                if (usuario == null)
                {
                    throw new EntityNotFoundException(nameof(Usuario), contextUsuarioId);
                }

                usuario.FcmToken = command.Token;

                _context.Usuarios.Update(usuario);

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
