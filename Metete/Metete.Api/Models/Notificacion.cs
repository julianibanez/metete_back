using System;
using System.Collections.Generic;

namespace Metete.Api.Models;

public partial class Notificacion
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public int IdTipoNotificacion { get; set; }

    public string Titulo { get; set; } = null!;

    public string Mensaje { get; set; } = null!;

    public int? IdEvento { get; set; }

    public int IdEstadoNotificacion { get; set; }

    public DateTime? FechaEnvio { get; set; }

    public bool Leido { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual Evento? Evento { get; set; } = null!;

    public virtual TipoNotificacion TipoNotificacion { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
