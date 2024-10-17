using AutoMapper;
using Metete.Api.Features.CategoriaGeneros.Queries;
using Metete.Api.Models;

namespace Metete.Api.Features.CategoriaGeneros
{
    public class CategoriaGenerosMappingProfile: Profile
    {
        public CategoriaGenerosMappingProfile()
        {
            CreateMap<CategoriaGenero, GetAllCategoriaGenerosForDropDownList.CategoriaGeneroDto>();
        }
    }
}
