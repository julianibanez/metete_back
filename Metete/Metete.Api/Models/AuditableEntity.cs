using System.ComponentModel.DataAnnotations;

namespace Metete.Api.Models
{
    public abstract class AuditableEntity
    {
        [StringLength(50)]
        public string Creador { get; set; } = null!;

        public DateTime FechaCreacion { get; set; }

        [StringLength(50)]
        public string Modificador { get; set; } = null!;

        public DateTime FechaModificacion { get; set; }
    }
}