namespace Metete.Api.Models;

public partial class UsuarioTipoDeporte
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdTipoDeporte { get; set; }

    public virtual TipoDeporte TipoDeporte { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
