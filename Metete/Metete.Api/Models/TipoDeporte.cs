namespace Metete.Api.Models;

public partial class TipoDeporte
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public bool RequierePosicion { get; set; }

    public virtual ICollection<Evento> Eventos { get; set; } = new List<Evento>();

    public virtual ICollection<UsuarioTipoDeporte> UsuarioTipoDeportes { get; set; } = new List<UsuarioTipoDeporte>();
}
