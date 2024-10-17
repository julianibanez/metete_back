using AutoMapper;
using Metete.Api.Features.Comunas.Queries;
using Metete.Api.Models;

namespace Metete.Api.Features.Comunas
{
    public class ComunasMappingProfile: Profile
    {
        public ComunasMappingProfile()
        {
            CreateMap<Comuna, GetAllComunasForDropDownList.ComunaDto>();
        }
    }
}
