namespace Metete.Api.Models;

public partial class UsuarioTipoNotificacion
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdTipoNotificacion { get; set; }

    public virtual TipoNotificacion TipoNotificacion { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
