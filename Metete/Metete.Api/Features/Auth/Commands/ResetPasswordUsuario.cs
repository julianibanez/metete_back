using MediatR;
using Metete.Api.Data;
using Metete.Api.Infraestructure.Constants;
using Metete.Api.Infraestructure.Exceptions;
using Metete.Api.Models;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;

namespace Metete.Api.Features.Auth.Commands
{
    public class ResetPasswordUsuario
    {
        public class Command : IRequest<Unit>
        {
            public string Username { get; set; } = null!;
            public string RecoveryCode { get; set; } = null!;
            public string NewPassword { get; set; } = null!;
            public string ConfirmNewPassword { get; set; } = null!;
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
                Usuario? usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(x => x.Username == command.Username, cancellationToken);

                if (usuario == null)
                {
                    throw new LoginFailedException(Feedback.USUARIO_NOT_FOUND);
                }

                if (usuario.RecoveryCode != command.RecoveryCode || DateTime.UtcNow > usuario.RecoveryCodeExpiryTime)
                {
                    throw new PasswordRecoveryException("Código de recuperación no válido");
                }

                if (command.NewPassword != command.ConfirmNewPassword)
                {
                    throw new PasswordRecoveryException("La contraseña no coincide");
                }

                usuario.Password = BC.HashPassword(command.NewPassword);
                usuario.RecoveryCode = null;

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}

