using AutoMapper;
using Metete.Api.Features.Auth.Commands;
using Metete.Api.Models;

namespace Metete.Api.Features.TipoDeportes
{
    public class AuthMappingProfile : Profile
    {
        public AuthMappingProfile()
        {
            // Commands
            CreateMap<RegisterUsuario.Command, Usuario>();
        }
    }
}
