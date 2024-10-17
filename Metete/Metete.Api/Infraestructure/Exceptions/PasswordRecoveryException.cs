namespace Metete.Api.Infraestructure.Exceptions
{
    public class PasswordRecoveryException : Exception
    {
        public PasswordRecoveryException()
        {
        }

        public PasswordRecoveryException(string message)
           : base(message)
        {
        }

        public PasswordRecoveryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

