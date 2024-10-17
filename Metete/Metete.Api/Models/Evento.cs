using NetTopologySuite.Geometries;

namespace Metete.Api.Models;

public partial class Evento: AuditableEntity
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int IdTipoDeporte { get; set; }

    public string? Descripcion { get; set; }

    public DateTime FechaEvento { get; set; }

    public string ZonaHorariaEvento { get; set; } = null!;

    //public int DiaDeLaSemana { get; set; }

    //public TimeSpan HoraEvento { get; set; }

    public int Duracion { get; set; }

    public int? IdCentroDeporte { get; set; }

    public bool OtroCentroDeporte { get; set; }

    public string? NombreCentroDeporte { get; set; }

    public string? DireccionCentroDeporte { get; set; }

    public decimal? LatitudCentroDeporte { get; set; }

    public decimal? LongitudCentroDeporte { get; set; }

    public Point? UbicacionCentroDeporte { get; set; }

    public int NumJugadores { get; set; }

    public decimal PrecioPorPersona { get; set; }

    public int IdMetodoPago { get; set; }

    public int IdTipoDivisa { get; set; }

    public bool DevolucionAbandono { get; set; }

    public bool RecordarEventoJugador { get; set; }

    public bool ObligatorioDisponerTelefono { get; set; }

    public int IdCategoriaGeneroPermitido { get; set; }

    public int IdCreador { get; set; }
    

    public virtual CategoriaGenero CategoriaGenero { get; set; } = null!;

    public virtual CentroDeporte CentroDeporte { get; set; } = null!;

    public virtual Usuario UsuarioCreador { get; set; } = null!;

    public virtual MetodoPago MetodoPago { get; set; } = null!;

    public virtual TipoDeporte TipoDeporte { get; set; } = null!;

    public virtual TipoDivisa TipoDivisa { get; set; } = null!;

    public virtual ICollection<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();

    public virtual ICollection<UsuarioEvaluacion> Evaluaciones { get; set; } = new List<UsuarioEvaluacion>();

    public virtual ICollection<UsuarioEvento> Usuarios { get; set; } = new List<UsuarioEvento>();
}
