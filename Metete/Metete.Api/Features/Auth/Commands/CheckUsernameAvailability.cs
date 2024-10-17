using MediatR;
using Metete.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Metete.Api.Features.Auth.Commands
{
    public class CheckUsernameAvailability
    {
        public class Command : IRequest<bool>
        {
            public string Username { get; set; } = null!;
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
                bool isTaken = await _context.Usuarios.AnyAsync(x => x.Username == command.Username, cancellationToken);

                return !isTaken;
            }
        }
    }
}
