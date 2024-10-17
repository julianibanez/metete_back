using MediatR;
using Metete.Api.Data;
using Metete.Api.Enums;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Metete.Api.Features.Notificaciones.Queries
{
    public class CountUnreadedNotificacionesByUsuario
    {
        public class Query : IRequest<int>
        {
        }

        public class Handler : IRequestHandler<Query, int>
        {
            private readonly MeteteContext _context;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(MeteteContext context, IHttpContextAccessor httpContextAccessor)
            {
                _context = context;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<int> Handle(Query query, CancellationToken cancellationToken)
            {
                int contextUsuarioId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                int count = await _context.Notificaciones
                    .CountAsync(x => x.IdEstadoNotificacion == (int)EstadoNotificacion.Enviada
                        && x.IdUsuario == contextUsuarioId
                        && !x.Leido, cancellationToken);


                return count;
            }
        }
    }
}