using NetTopologySuite.Geometries;

namespace Metete.Api.Models;

public partial class Comuna
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int IdRegion { get; set; }

    public decimal Latitud { get; set; }

    public decimal Longitud { get; set; }

    public Point Ubicacion { get; set; } = null!;

    public virtual ICollection<CentroDeporte> CentroDeportes { get; set; } = new List<CentroDeporte>();    

    public virtual Region Region { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

    public virtual ICollection<UsuarioComuna> UsuarioComunas { get; set; } = new List<UsuarioComuna>();
}
