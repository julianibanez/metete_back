namespace Metete.Api.Models;

public partial class CriterioMvp
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<UsuarioEvaluacion> UsuarioEvaluaciones { get; set; } = new List<UsuarioEvaluacion>();
}
