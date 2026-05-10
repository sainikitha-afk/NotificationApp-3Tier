using System;
using NotificationApp.Interfaces;
using NotificationApp.Models;

namespace NotificationApp.NotificationSenders
{
    internal class SmsNotificationSender : INotificationSender
    {
        public void Send(User user, Notification notification)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(notification);

            Console.WriteLine();
            Console.WriteLine(" SMS NOTIFICATION ");
            Console.WriteLine($"To          : {user.Phone}");
            Console.WriteLine($"Receiver    : {user.Name}");
            Console.WriteLine("#################################");
            Console.WriteLine(notification.Message);
            Console.WriteLine("#################################");
            Console.WriteLine($"Delivered At : {notification.SentDate:dd MMM yyyy hh:mm tt}");
            Console.WriteLine();
        }
    }
}
