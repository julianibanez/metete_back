namespace Metete.Api.Models;

public partial class UsuarioHorario
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public int DiaDeLaSemana { get; set; }

    public TimeSpan HorarioInicio { get; set; }

    public TimeSpan HorarioTermino { get; set; }

    public string ZonaHoraria { get; set; } = null!;

    public bool Disponible { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}
