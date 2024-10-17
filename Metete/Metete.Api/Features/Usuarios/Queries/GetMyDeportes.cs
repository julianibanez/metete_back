using AutoMapper;
using MediatR;
using Metete.Api.Data;
using Metete.Api.Infraestructure.Exceptions;
using Metete.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Metete.Api.Features.Usuarios.Queries
{
    public class GetMyDeportes
    {
        public class Query : IRequest<List<int>>
        {          
        }

        public class Handler : IRequestHandler<Query, List<int>>
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

            public async Task<List<int>> Handle(Query query, CancellationToken cancellationToken)
            {
                int contextUsuarioId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                Usuario? usuario = await _context.Usuarios
                    .Include(x => x.UsuarioTipoDeportes)
                    .FirstOrDefaultAsync(x => x.Id == contextUsuarioId, cancellationToken);

                if (usuario == null)
                {
                    throw new EntityNotFoundException(nameof(Usuario), contextUsuarioId);
                }

                List<int> result = usuario.UsuarioTipoDeportes.Select(x => x.IdTipoDeporte).ToList();

                return result;
            }
        }
    }
}