namespace Metete.Api.Models;

public partial class TipoNotificacion
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Titulo { get; set; } = null!;

    public string Mensaje { get; set; } = null!;

    public virtual ICollection<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();

    public virtual ICollection<UsuarioTipoNotificacion> UsuarioTipoNotificaciones { get; set; } = new List<UsuarioTipoNotificacion>();
}
