namespace Metete.Api.Infraestructure.Exceptions
{
    public class DBOperationFailedException : Exception
    {
        public DBOperationFailedException()
        {
        }

        public DBOperationFailedException(string message)
            : base(message)
        {
        }

        public DBOperationFailedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

