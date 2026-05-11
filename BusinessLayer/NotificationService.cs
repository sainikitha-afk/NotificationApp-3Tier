using System;
using NotificationApp.Interfaces;
using NotificationApp.Models;
using NotificationApp.NotificationSenders;

namespace NotificationApp.BusinessLayer
{
    internal class NotificationService
    {
        private readonly IRepository repo; // repository for storing sent notifications - intiatlizing repo object of interface IRepository

        public NotificationService(IRepository repository) // dependency injection -  
        {
            repo = repository;
        }

        public bool SendNotification(User user, Notification notification, string type)
        {
            if (user == null)
            {
                throw new NotificationValidationException("User information is required.");
            }

            if (notification == null)
            {
                throw new NotificationValidationException("Notification object is required.");
            }

            var mode = NormalizeType(type);
            ValidateType(mode);
            ValidateUser(user, mode);
            ValidateMessage(notification);
            ApplyRules(mode, notification);

            var sender = GetSender(mode) ?? throw new NotificationValidationException("Unsupported notification type.");
            notification.SentDate = DateTime.Now;

            var storedUser = repo.GetUser(user.Email, user.Phone);
            if (storedUser == null)
            {
                repo.AddUser(user);
                storedUser = user;
            }

            notification.Sender = storedUser;

            try
            {
                sender.Send(storedUser, notification);
                repo.Add(notification);
                return true;
            }
            catch (Exception ex) when (ex is not NotificationValidationException)
            {
                throw new NotificationProcessException("Failed to send notification.", ex);
            }
        }

        public bool UpdateNotification(int index, string message)
        {
            if (index < 0)
            {
                throw new NotificationValidationException("Index must be a non-negative number.");
            }

            var existing = repo.GetByIndex(index);
            if (existing == null)
            {
                throw new NotificationValidationException("Notification not found.");
            }

            var updated = new Notification
            {
                Message = message,
                SentDate = DateTime.Now,
                Sender = existing.Sender
            };

            ValidateMessage(updated);
            repo.Update(index, updated);
            return true;
        }

        private static string NormalizeType(string type)
        {
            return type?.Trim().ToLowerInvariant() ?? string.Empty;
        }

        private static void ValidateType(string type)
        {
            if (type != "email" && type != "sms")
            {
                throw new NotificationValidationException("Notification type must be either email or sms.");
            }
        }

        private static void ValidateUser(User user, string type)
        {
            if (string.IsNullOrWhiteSpace(user.Name))
            {
                throw new NotificationValidationException("User name is required.");
            }

            if (type == "email" && !user.HasValidEmail())
            {
                throw new NotificationValidationException("A valid email address is required for email notifications.");
            }

            if (type == "sms" && !user.HasValidPhone())
            {
                throw new NotificationValidationException("A valid phone number is required for SMS notifications.");
            }
        }

        private static void ValidateMessage(Notification notification)
        {
            if (!notification.HasContent())
            {
                throw new NotificationValidationException("Notification message cannot be blank.");
            }
        }

        private static void ApplyRules(string type, Notification notification)
        {
            if (type == "sms" && notification.Message.Length > 160)
            {
                throw new NotificationValidationException("SMS messages must be 160 characters or fewer.");
            }

            if (type == "email" && notification.Message.Length > 1000)
            {
                throw new NotificationValidationException("Email messages must be 1000 characters or fewer.");
            }
        }

        private static INotificationSender? GetSender(string type)
        {
            return type switch
            {
                "email" => new EmailNotificationSender(),
                "sms" => new SmsNotificationSender(),
                _ => null,
            };
        }
    }
}
