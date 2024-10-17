using MediatR;
using Metete.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Metete.Api.Features.Usuarios.Queries
{
    public class GetMyRecord
    {
        public class Query : IRequest<RecordDto>
        {          
        }

        public class Handler : IRequestHandler<Query, RecordDto>
        {
            private readonly MeteteContext _context;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(MeteteContext context, IHttpContextAccessor httpContextAccessor)
            {
                _context = context;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<RecordDto> Handle(Query query, CancellationToken cancellationToken)
            {
                int contextUsuarioId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                int numOrganizados = await _context.Eventos                    
                    .CountAsync(x => x.IdCreador == contextUsuarioId, cancellationToken);

                int numJugados = await _context.UsuarioEventos
                    .CountAsync(x => x.IdUsuario == contextUsuarioId, cancellationToken);

                return new RecordDto { NumOrganizados = numOrganizados, NumJugados = numJugados};
            }
        }

        public class RecordDto
        {
            public int NumOrganizados { get; set; }

            public int NumJugados { get; set; }
        }
    }
}