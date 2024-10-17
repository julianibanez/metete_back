namespace Metete.Api.Models;

public partial class UsuarioComuna
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdComuna { get; set; }

    public virtual Comuna Comuna { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
