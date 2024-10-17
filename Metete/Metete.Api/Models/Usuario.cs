namespace Metete.Api.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool Administrador { get; set; }

    public string Nombre { get; set; } = null!;

    public string ApellidoPaterno { get; set; } = null!;

    public string ApellidoMaterno { get; set; } = null!;

    public DateTime FechaNacimiento { get; set; }

    public int IdTipoGenero { get; set; }

    public int CodigoPais { get; set; }

    public string Telefono { get; set; } = null!;

    public int IdPaisResidencia { get; set; }

    public int IdNacionalidad { get; set; }

    public string Direccion { get; set; } = null!;

    public int IdComuna { get; set; }

    public decimal Latitud { get; set; }

    public decimal Longitud { get; set; }

    public int IdTipoMembresia { get; set; }

    public bool Activo { get; set; }

    public bool Eliminado { get; set; }

    public string? FcmToken { get; set; }

    public string? RecoveryCode { get; set; }

    public DateTime? RecoveryCodeExpiryTime { get; set; }

    public int? KmBusqueda { get; set; }

    public string? Foto { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryTime { get; set; }

    public virtual ICollection<Evento> Eventos { get; set; } = new List<Evento>();

    public virtual Comuna Comuna { get; set; } = null!;

    public virtual Nacionalidad Nacionalidad { get; set; } = null!;

    public virtual Pais PaisResidencia { get; set; } = null!;

    public virtual TipoGenero TipoGenero { get; set; } = null!;

    public virtual TipoMembresia TipoMembresia { get; set; } = null!;    

    public virtual ICollection<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();

    public virtual ICollection<UsuarioComuna> UsuarioComunas { get; set; } = new List<UsuarioComuna>();

    public virtual ICollection<UsuarioEvaluacion> UsuarioEvaluaciones { get; set; } = new List<UsuarioEvaluacion>();

    public virtual ICollection<UsuarioEvento> UsuarioEventos { get; set; } = new List<UsuarioEvento>();

    public virtual ICollection<UsuarioHorario> UsuarioHorarios { get; set; } = new List<UsuarioHorario>();

    public virtual ICollection<UsuarioTipoDeporte> UsuarioTipoDeportes { get; set; } = new List<UsuarioTipoDeporte>();

    public virtual ICollection<UsuarioTipoNotificacion> UsuarioTipoNotificaciones { get; set; } = new List<UsuarioTipoNotificacion>();
}
