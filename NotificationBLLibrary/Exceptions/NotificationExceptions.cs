namespace NotificationBLLibrary.Exceptions
{
    public class NotificationValidationException : Exception
    {
        public NotificationValidationException(string message)
            : base(message)
        {
        }
    }

    public class NotificationProcessException : Exception
    {
        public NotificationProcessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}