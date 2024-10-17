using AutoMapper;
using Metete.Api.Features.Paises.Queries;
using Metete.Api.Models;

namespace Metete.Api.Features.Paises
{
    public class PaisesMappingProfile: Profile
    {
        public PaisesMappingProfile()
        {
            CreateMap<Pais, GetAllPaisesForDropDownList.PaisDto>();
        }
    }
}
