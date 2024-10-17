namespace Metete.Api.Models;

public partial class Rol
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<UsuarioEvento> UsuarioEventos { get; set; } = new List<UsuarioEvento>();
}
