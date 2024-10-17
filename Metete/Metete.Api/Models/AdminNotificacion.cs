using System;
using System.Collections.Generic;

namespace Metete.Api.Models;

public partial class AdminNotificacion
{
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string Mensaje { get; set; } = null!;

    public int IdEstado { get; set; }

    public DateTime FechaCreacion { get; set; }
}
