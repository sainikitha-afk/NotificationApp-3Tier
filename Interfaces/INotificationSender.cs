using NotificationApp.Models;

namespace NotificationApp.Interfaces
{
    // defines the contract for sending notifications.
    // mplementations will handle different notification channels like email or SMS. -> runtime polymorphism  will be implemented in EmailNotificationSender and SmsNotificationSender classes.
    internal interface INotificationSender 
    {
        // sends a notification to the specified user.
        public void Send(User user, Notification notification); // only method declaration - abstraction, no 
    }
}
