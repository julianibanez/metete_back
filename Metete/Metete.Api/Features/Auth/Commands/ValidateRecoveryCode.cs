using MediatR;
using Metete.Api.Data;
using Metete.Api.Infraestructure.Constants;
using Metete.Api.Infraestructure.Exceptions;
using Metete.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Metete.Api.Features.Auth.Commands
{
    public class ValidateRecoveryCode
    {
        public class Command : IRequest<bool>
        {
            public string Username { get; set; } = null!;
            public string RecoveryCode { get; set; } = null!;
        }

        public class Handler : IRequestHandler<Command, bool>
        {
            private readonly MeteteContext _context;

            public Handler(MeteteContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(Command command, CancellationToken cancellationToken)
            {
                Usuario? usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(x => x.Username == command.Username, cancellationToken);

                if (usuario == null)
                {
                    throw new LoginFailedException(Feedback.USUARIO_NOT_FOUND);
                }

                if (usuario.RecoveryCode == command.RecoveryCode)
                {
                    if (DateTime.UtcNow <= usuario.RecoveryCodeExpiryTime)
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }
}

