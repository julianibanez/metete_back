namespace Metete.Api.Models;

public partial class UsuarioEvaluacion
{
    public int IdUsuarioEvaluacion { get; set; }

    public int IdEvento { get; set; }

    public int IdEvaluacion { get; set; }

    public int IdCriterioMvp { get; set; }

    public int IdUsuario { get; set; }

    public int IdEvaluador { get; set; }

    public int Nota { get; set; }

    public virtual CriterioMvp CriterioMvp { get; set; } = null!;

    public virtual Evento Evento { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
