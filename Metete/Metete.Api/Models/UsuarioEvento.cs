namespace Metete.Api.Models;

public partial class UsuarioEvento
{
    public int Id { get; set; }

    public int IdEvento { get; set; }

    public int IdUsuario { get; set; }

    public int IdRol { get; set; }

    public bool? Aprobado { get; set; }

    public string? Posicion { get; set; }

    public virtual Evento Evento { get; set; } = null!;

    public virtual Rol Rol { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
