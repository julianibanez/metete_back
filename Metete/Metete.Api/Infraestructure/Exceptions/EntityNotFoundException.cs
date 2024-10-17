namespace Metete.Api.Infraestructure.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string message)
            : base(message)
        {
        }

        public EntityNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public EntityNotFoundException(string name, object key)
            : base($"Entidad {name} ({key}) no fue encontrada.")
        {
        }
    }
}
