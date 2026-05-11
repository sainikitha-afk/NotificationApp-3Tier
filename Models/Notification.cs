using System;

namespace NotificationApp.Models
{
    // represents a notification that has been sent
    // contains the message content and the date it was sent
    internal class Notification
    {
        public int NotId { get; set; }

        // notification type
        public string NotType { get; set; } = string.Empty;

        // notification message
        public string Message { get; set; } = string.Empty;

        // sent time
        public DateTime SentDate { get; set; }

        // sender details
        public User Sender { get; set; } = new User();
        public string GetAuditLine()
        {
            return $"[{SentDate:dd-MM-yyyy hh:mm tt}] {Sender.GetShortInfo()} | {Message}";
        }

        // checks if the notification has any content
        public bool HasContent()
        {
            return !string.IsNullOrWhiteSpace(Message);
        }
    }
}
