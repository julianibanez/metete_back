using NetTopologySuite.Geometries;

namespace Metete.Api.Models;

public partial class CentroDeporte
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public int IdComuna { get; set; }

    public decimal Latitud { get; set; }

    public decimal Longitud { get; set; }

    public Point Ubicacion { get; set; } = null!;

    public bool Aprobado { get; set; }

    public virtual ICollection<Evento> Eventos { get; set; } = new List<Evento>();

    public virtual Comuna Comuna { get; set; } = null!;
}
