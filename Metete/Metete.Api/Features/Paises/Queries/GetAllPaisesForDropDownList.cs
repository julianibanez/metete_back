using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Metete.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Metete.Api.Features.Paises.Queries
{
    public class GetAllPaisesForDropDownList
    {
        public class Query : IRequest<List<PaisDto>>
        {
        }

        public class Handler : IRequestHandler<Query, List<PaisDto>>
        {
            private readonly MeteteContext _context;
            private readonly IMapper _mapper;

            public Handler(MeteteContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<PaisDto>> Handle(Query query, CancellationToken cancellationToken)
            {
                List<PaisDto> oaises = await _context.Paises.AsNoTracking()
                     .OrderBy(x => x.Nombre)
                     .ProjectTo<PaisDto>(_mapper.ConfigurationProvider)
                     .ToListAsync();

                return oaises;
            }
        }

        public class PaisDto
        {
            public int Id { get; set; }

            public string Nombre { get; set; } = null!;
        }
    }
}
