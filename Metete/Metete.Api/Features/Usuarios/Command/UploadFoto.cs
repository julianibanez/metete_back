using MediatR;
using Metete.Api.Data;
using Metete.Api.Infraestructure.Exceptions;
using Metete.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Metete.Api.Features.Usuarios.Command
{
    public class UploadFoto
    {
        public class Command : IRequest<FotoDto>
        {
            public IFormFile Foto { get; set; } = null!;
        }

        public class Handler : IRequestHandler<Command, FotoDto>
        {
            private readonly MeteteContext _context;
            private readonly IConfiguration _configuration;
            private readonly IHostEnvironment _env;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(MeteteContext context, IConfiguration configuration, IHostEnvironment env, IHttpContextAccessor httpContextAccessor)
            {
                _context = context;
                _configuration = configuration;
                _env = env;
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<FotoDto> Handle(Command command, CancellationToken cancellationToken)
            {
                if (command.Foto.Length <= 0)
                {
                    throw new BusinessRuleException("No se ha adjuntado una imagen");
                }

                int contextUsuarioId = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

                Usuario? usuario = await _context.Usuarios
                    .Include(x => x.UsuarioHorarios)
                    .FirstOrDefaultAsync(x => x.Id == contextUsuarioId, cancellationToken);

                if (usuario == null)
                {
                    throw new EntityNotFoundException(nameof(Usuario), contextUsuarioId);
                }

                string? oldFoto = usuario.Foto;

                string fileName = @$"{Guid.NewGuid().ToString().Replace("-", "")}.png";
                string filePath = Path.Combine(_configuration["UserFilesBasePath"]!, "fotos", fileName);

                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    await command.Foto.CopyToAsync(fs, cancellationToken);
                }

                usuario.Foto = fileName;

                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync(cancellationToken);

                // Elimina foto anterior
                if (!string.IsNullOrWhiteSpace(oldFoto))
                {
                    string oldFilePath = Path.Combine(_configuration["UserFilesBasePath"]!, "fotos", oldFoto);

                    try
                    {
                        if (File.Exists(oldFilePath))
                        {
                            File.Delete(oldFilePath);
                        }
                    }
                    catch { }
                }

                var fotoDto = new FotoDto
                {
                    Nombre = fileName,
                };

                return fotoDto;
            }
        }

        public class FotoDto
        {
            public string Nombre { get; set; } = null!;
        }
    }
}
