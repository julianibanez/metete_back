namespace Metete.WorkerService.Dtos
{
    public class NotificacionDto
    {
        public int Id { get; set; }

        public string FcmToken { get; set; } = null!;

        public string Titulo { get; set; } = null!;

        public string Mensaje { get; set; } = null!;

        public int? IdEvento { get; set; }

        public int? IdTipoNotificacion { get; set; }
    }
}
