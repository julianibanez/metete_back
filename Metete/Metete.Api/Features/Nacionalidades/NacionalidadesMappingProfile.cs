using AutoMapper;
using Metete.Api.Features.Nacionalidades.Queries;
using Metete.Api.Models;

namespace Metete.Api.Features.Nacionalidades
{
    public class NacionalidadesMappingProfile: Profile
    {
        public NacionalidadesMappingProfile()
        {
            CreateMap<Nacionalidad, GetAllNacionalidadesForDropDownList.NacionalidadDto>();
        }
    }
}
