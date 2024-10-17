namespace Metete.Api.Models;

public partial class DetalleFuncionalidad
{
    public int Id { get; set; }

    public int IdFuncionalidad { get; set; }

    public string NombreFormulario { get; set; } = null!;

    public string Codigo { get; set; } = null!;

    public virtual Funcionalidad Funcionalidad { get; set; } = null!;
}
