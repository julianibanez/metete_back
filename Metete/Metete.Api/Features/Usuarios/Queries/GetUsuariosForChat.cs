using AutoMapper;
using MediatR;
using Metete.Api.Data;
using Metete.Api.Infraestructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Metete.Api.Features.Usuarios.Queries
{
    public class GetUsuariosForChat
    {
        public class Query : IRequest<List<UsuarioDto>>
        {
            public string Ids { get; set; } = null!;
        }

        public class Handler : IRequestHandler<Query, List<UsuarioDto>>
        {
            private readonly MeteteContext _context;
            private readonly IMapper _mapper;

            public Handler(MeteteContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<UsuarioDto>> Handle(Query query, CancellationToken cancellationToken)
            {
                List<int> ids = query.Ids.Split(',').Select(int.Parse).ToList();

                List<UsuarioDto> usuarios = await _context.Usuarios
                    .AsNoTracking()
                    .Where(x => ids.Contains(x.Id))
                    .ProjectToListAsync<UsuarioDto>(_mapper.ConfigurationProvider);

                return usuarios;
            }
        }

        public class UsuarioDto
        {
            public int Id { get; set; }

            public string Nombre { get; set; } = null!;
        }
    }
}