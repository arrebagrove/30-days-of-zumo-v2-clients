using System;

namespace UWP.Services
{
    public class AzureMobileAppException : Exception
    {
        public AzureMobileAppException()
        {
        }

        public AzureMobileAppException(string message) : base(message)
        {
        }

        public AzureMobileAppException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}