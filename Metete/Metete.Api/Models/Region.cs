namespace Metete.Api.Models;

public partial class Region
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Comuna> Comunas { get; set; } = new List<Comuna>();
}
