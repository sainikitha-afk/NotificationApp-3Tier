using System;

namespace NotificationApp.BusinessLayer
{
    internal class NotificationValidationException : Exception
    {
        public NotificationValidationException(string message)
            : base(message)
        {
        }
    }

    internal class NotificationProcessException : Exception
    {
        public NotificationProcessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
