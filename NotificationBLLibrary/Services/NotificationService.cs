using NotificationBLLibrary.Exceptions;
using NotificationBLLibrary.Interfaces;
using NotificationDALLibrary.Repositories;
using NotificationModelLibrary;
using NotificationSenderLibrary;
using NotificationSenderLibrary.Interfaces;

namespace NotificationBLLibrary.Services
{
    public class NotificationService : INotificationService
    {
        private readonly NotificationRepository notificationRepository;

        private readonly UserRepository userRepository;

        public NotificationService()
        {
            notificationRepository = new NotificationRepository();

            userRepository = new UserRepository();
        }

        public bool SendNotification(
            User user,
            Notification notification,
            string type
        )
        {
            if (user == null)
            {
                throw new NotificationValidationException(
                    "User information is required."
                );
            }

            if (notification == null)
            {
                throw new NotificationValidationException(
                    "Notification object is required."
                );
            }

            var mode = NormalizeType(type);

            ValidateType(mode);

            ValidateUser(user, mode);

            ValidateMessage(notification);

            ApplyRules(mode, notification);

            var sender =
                GetSender(mode)
                ?? throw new NotificationValidationException(
                    "Unsupported notification type."
                );

            notification.SentDate = DateTime.Now;

            notification.NotType = mode;

            var storedUser =
                userRepository.GetByContact(user.Email, user.Phone);

            if (storedUser == null)
            {
                storedUser = userRepository.Add(user);
            }

            notification.UserId = storedUser.UserId;

            try
            {
                sender.Send(storedUser, notification);

                notificationRepository.Add(notification);

                return true;
            }
            catch (Exception ex)
                when (ex is not NotificationValidationException)
            {
                throw new NotificationProcessException(
                    "Failed to send notification.",
                    ex
                );
            }
        }

        public bool UpdateNotification(int id, string message)
        {
            if (id < 0)
            {
                throw new NotificationValidationException(
                    "Id must be non-negative."
                );
            }

            var existing =
                notificationRepository.Get(id);

            if (existing == null)
            {
                throw new NotificationValidationException(
                    "Notification not found."
                );
            }

            existing.Message = message;

            existing.SentDate = DateTime.Now;

            ValidateMessage(existing);

            notificationRepository.Update(id, existing);

            return true;
        }

        private static string NormalizeType(string type)
        {
            return type?.Trim().ToLowerInvariant()
                   ?? string.Empty;
        }

        private static void ValidateType(string type)
        {
            if (type != "email" && type != "sms")
            {
                throw new NotificationValidationException(
                    "Notification type must be email or sms."
                );
            }
        }

        private static void ValidateUser(User user, string type)
        {
            if (string.IsNullOrWhiteSpace(user.Name))
            {
                throw new NotificationValidationException(
                    "User name is required."
                );
            }

            if (type == "email" && !user.HasValidEmail())
            {
                throw new NotificationValidationException(
                    "Valid email required."
                );
            }

            if (type == "sms" && !user.HasValidPhone())
            {
                throw new NotificationValidationException(
                    "Valid phone number required."
                );
            }
        }

        private static void ValidateMessage(Notification notification)
        {
            if (!notification.HasContent())
            {
                throw new NotificationValidationException(
                    "Message cannot be empty."
                );
            }
        }

        private static void ApplyRules(
            string type,
            Notification notification
        )
        {
            if (type == "sms"
                && notification.Message.Length > 160)
            {
                throw new NotificationValidationException(
                    "SMS cannot exceed 160 characters."
                );
            }

            if (type == "email"
                && notification.Message.Length > 1000)
            {
                throw new NotificationValidationException(
                    "Email cannot exceed 1000 characters."
                );
            }
        }

        private static INotificationSender? GetSender(string type)
        {
            return type switch
            {
                "email" => new EmailNotificationSender(),
                "sms" => new SmsNotificationSender(),
                _ => null
            };
        }
    }
}