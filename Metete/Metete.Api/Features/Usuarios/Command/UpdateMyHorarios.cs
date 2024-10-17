using AutoMapper;
using MediatR;
using Metete.Api.Data;
using Metete.Api.Infraestructure.Exceptions;
using Metete.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Metete.Api.Features.Usuarios.Command
{
    public class UpdateMyHorarios
    {
        public class Command : IRequest<Unit>
        {
            public List<UsuarioHorarioDto> Horarios { get; set; } = new();
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly MeteteContext _context;
            private readonly IMapper _mapper;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(MeteteContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
            {
                _context = context;
                _mapper = mapper;
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

                // Horarios nuevos
                var newHorarios = command.Horarios.Where(x => !usuario.UsuarioHorarios.Any(y =>
                    y.DiaDeLaSemana == x.DiaDeLaSemana
                    && y.HorarioInicio == TimeSpan.Parse(x.HorarioInicio)
                    && y.HorarioTermino == TimeSpan.Parse(x.HorarioTermino)))
                    .Select(x => new UsuarioHorario
                    {
                        DiaDeLaSemana = x.DiaDeLaSemana,
                        HorarioInicio = TimeSpan.Parse(x.HorarioInicio),
                        HorarioTermino = TimeSpan.Parse(x.HorarioTermino),
                        ZonaHoraria = x.ZonaHoraria,
                    }).ToList();

                // Agrega los horarios nuevos
                newHorarios.ForEach(x => usuario.UsuarioHorarios.Add(x));

                _context.Usuarios.Update(usuario);

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }

        public class UsuarioHorarioDto
        {
            public int DiaDeLaSemana { get; set; }

            public string HorarioInicio { get; set; } = null!;

            public string HorarioTermino { get; set; } = null!;

            public string ZonaHoraria { get; set; } = null!;
        }
    }
}