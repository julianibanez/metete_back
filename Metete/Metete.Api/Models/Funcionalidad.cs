namespace Metete.Api.Models;

public partial class Funcionalidad
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Codigo { get; set; } = null!;

    public bool EsDePago { get; set; }

    public virtual ICollection<DetalleFuncionalidad> DetalleFuncionalidades { get; set; } = new List<DetalleFuncionalidad>();
}
