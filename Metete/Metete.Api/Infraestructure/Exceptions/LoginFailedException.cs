﻿namespace Metete.Api.Infraestructure.Exceptions
{
    public class LoginFailedException : Exception
    {
        public LoginFailedException()
        {
        }

        public LoginFailedException(string message)
            : base(message)
        {
        }

        public LoginFailedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
