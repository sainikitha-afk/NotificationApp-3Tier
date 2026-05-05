using NotificationApp.Interfaces;
using NotificationApp.Models;

namespace NotificationApp.Services
{
    // class that acts as a messenger to send notifications
    internal class NotificationService
    {
        // Takes any notification type (Email/SMS/etc) and sends it
        public void SendNotification(INotification notifier, User user, Notification notification)
        {
            // INotification Notifier - interphase implements polymorphism
            notifier.Send(user, notification);
        }
    }
}