using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Metete.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Metete.Api.Features.Nacionalidades.Queries
{
    public class GetAllNacionalidadesForDropDownList
    {
        public class Query : IRequest<List<NacionalidadDto>>
        {
        }

        public class Handler : IRequestHandler<Query, List<NacionalidadDto>>
        {
            private readonly MeteteContext _context;
            private readonly IMapper _mapper;

            public Handler(MeteteContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<NacionalidadDto>> Handle(Query query, CancellationToken cancellationToken)
            {
                List<NacionalidadDto> nacionalidades = await _context.Nacionalidades.AsNoTracking()
                     .OrderBy(x => x.Nombre)
                     .ProjectTo<NacionalidadDto>(_mapper.ConfigurationProvider)
                     .ToListAsync();

                return nacionalidades;
            }
        }

        public class NacionalidadDto
        {
            public int Id { get; set; }

            public string Nombre { get; set; } = null!;
        }
    }
}
