using AutoMapper;
using Metete.Api.Features.TipoGeneros.Queries;
using Metete.Api.Models;

namespace Metete.Api.Features.TipoGeneros
{
    public class TipoGenerosMappingProfile: Profile
    {
        public TipoGenerosMappingProfile()
        {
            CreateMap<TipoGenero, GetAllTipoGenerosForDropDownList.TipoGeneroDto>();
        }
    }
}
