using System;

namespace NotificationApp.Models
{
    // represents a notification that has been sent
    // contains the message content and the date it was sent
    internal class Notification
    {
        // gets or sets the message content of the notification
        public string Message { get; set; } = string.Empty;

        // gets or sets the date and time when the notification was sent
        public DateTime SentDate { get; set; }

        // gets a formatted audit line for logging purposes
        // includes the sent date and message
        public string GetAuditLine()
        {
            return $"[{SentDate:dd-MM-yyyy hh:mm tt}] {Message}";
        }

        // checks if the notification has any content
        public bool HasContent()
        {
            return !string.IsNullOrWhiteSpace(Message);
        }
    }
}
