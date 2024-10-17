using MediatR;
using Metete.Api.Data;
using Metete.Api.Infraestructure.Constants;
using Metete.Api.Infraestructure.Exceptions;
using Metete.Api.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Metete.Api.Features.Auth.Commands
{
    public class RefreshToken
    {
        public class Command : IRequest<AuthDataDto>
        {
            public string AccessToken { get; set; } = null!;
            public string RefreshToken { get; set; } = null!;
        }

        public class Handler : IRequestHandler<Command, AuthDataDto>
        {
            private readonly MeteteContext _context;
            private readonly ITokenService _tokenService;
            private readonly IConfiguration _configuration;

            public Handler(MeteteContext context, ITokenService tokenService, IConfiguration configuration)
            {
                _context = context;
                _tokenService = tokenService;
                _configuration = configuration;
            }

            public async Task<AuthDataDto> Handle(Command command, CancellationToken cancellationToken)
            {
                var principal = _tokenService.GetPrincipalFromExpiredToken(command.AccessToken);
                int id = int.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier)!);

                var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

                if (usuario == null)
                {
                    throw new LoginFailedException(Feedback.USUARIO_NOT_FOUND);
                }

                if (command.RefreshToken != usuario.RefreshToken || usuario.RefreshTokenExpiryTime < DateTime.UtcNow)
                {
                    throw new LoginFailedException(Feedback.REFRESH_TOKEN_NOT_VALID);
                }

                string accessToken = _tokenService.GenerateAccessToken(usuario);
                string refreshToken = _tokenService.GenerateRefreshToken();

                usuario.RefreshToken = refreshToken;
                usuario.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["RefreshToken:ExpiresIn"]!));

                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync(cancellationToken);

                var authData = new AuthDataDto()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                };

                return authData;
            }
        }

        public class AuthDataDto
        {
            public string AccessToken { get; set; } = null!;
            public string RefreshToken { get; set; } = null!;
        }
    }
}
