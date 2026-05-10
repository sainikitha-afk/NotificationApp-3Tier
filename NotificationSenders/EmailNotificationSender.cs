using System;
using NotificationApp.Interfaces;
using NotificationApp.Models;

namespace NotificationApp.NotificationSenders
{
    internal class EmailNotificationSender : INotificationSender
    {
        public void Send(User user, Notification notification)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(notification);

            Console.WriteLine();
            Console.WriteLine(" EMAIL NOTIFICATION ");
            Console.WriteLine($"To      : {user.Email}");
            Console.WriteLine($"Name    : {user.Name}");
            Console.WriteLine("################################");
            Console.WriteLine(notification.Message);
            Console.WriteLine("################################");
            Console.WriteLine($"Sent At : {notification.SentDate:dd MMM yyyy hh:mm tt}");
            Console.WriteLine();
        }
    }
}
