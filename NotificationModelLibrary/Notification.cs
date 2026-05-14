using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotificationModelLibrary
{
    public partial class Notification
    {
        public int NotId { get; set; }
        public string NotType { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime SentDate { get; set; }
        // foreign key
        public int UserId { get; set; }
        // fluentapi 
        public User? Sender { get; set; }
        public bool HasContent()
        {
            return !string.IsNullOrWhiteSpace(Message);
        }
    }
}