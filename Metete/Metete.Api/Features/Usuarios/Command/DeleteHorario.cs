using AutoMapper;
using MediatR;
using Metete.Api.Data;
using Metete.Api.Infraestructure.Exceptions;
using Metete.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Metete.Api.Features.Usuarios.Command
{
    public class DeleteHorario
    {
        public class Command : IRequest<Unit>
        {
            public int Id { get; set; }
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
                    .Include(x => x.UsuarioHorarios)
                    .FirstOrDefaultAsync(x => x.Id == contextUsuarioId, cancellationToken);

                if (usuario == null)
                {
                    throw new EntityNotFoundException(nameof(Usuario), contextUsuarioId);
                }

                UsuarioHorario? usuarioHorario = usuario.UsuarioHorarios.FirstOrDefault(x => x.Id == command.Id);

                if (usuarioHorario == null)
                {
                    throw new EntityNotFoundException(nameof(UsuarioHorario), command.Id);
                }

                _context.UsuarioHorarios.Remove(usuarioHorario);

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}