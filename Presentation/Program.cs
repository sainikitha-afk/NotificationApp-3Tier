using NotificationApp.BusinessLayer;
using NotificationApp.DataAccessLayer;
using NotificationApp.Interfaces;
using NotificationApp.Models;

namespace NotificationApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var repository = new NotificationRepository();
            var service = new NotificationService(repository);
            var running = true;

            while (running)
            {
                Console.WriteLine("\nMenu");
                Console.WriteLine("1. Send notification");
                Console.WriteLine("2. View all notifications");
                Console.WriteLine("3. View notification by index");
                Console.WriteLine("4. Update notification");
                Console.WriteLine("5. Delete notification");
                Console.WriteLine("6. View all users");
                Console.WriteLine("7. Exit");

                var input = Console.ReadLine();
                if (!int.TryParse(input, out var choice))
                {
                    Console.WriteLine("Enter a valid numeric choice.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        SendNotificationFlow(service);
                        break;

                    case 2:
                        DisplayAllNotifications(repository);
                        break;

                    case 3:
                        DisplayNotificationByIndex(repository);
                        break;

                    case 4:
                        UpdateNotificationFlow(service);
                        break;

                    case 5:
                        DeleteNotification(repository);
                        break;

                    case 6:
                        DisplayAllUsers(repository);
                        break;

                    case 7:
                        running = false;
                        Console.WriteLine("Application closed.");
                        break;

                    default:
                        Console.WriteLine("Choose a valid option.");
                        break;
                }
            }
        }

        private static void SendNotificationFlow(NotificationService service)
        {
            var type = ReadNotificationType();
            var user = ReadUser(type);
            var notification = ReadNotification();

            try
            {
                if (service.SendNotification(user, notification, type))
                {
                    Console.WriteLine("\nNotification sent successfully.");
                }
            }
            catch (NotificationValidationException ex)
            {
                Console.WriteLine($"Validation error: {ex.Message}");
            }
            catch (NotificationProcessException ex)
            {
                Console.WriteLine($"Send failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        private static void DisplayAllNotifications(IRepository repository)
        {
            var all = repository.GetAll();
            if (all.Count == 0)
            {
                Console.WriteLine("No notifications found.");
                return;
            }

            for (var i = 0; i < all.Count; i++)
            {
                Console.WriteLine($"Index {i} : {all[i].GetAuditLine()}");
            }
        }

        private static void DisplayAllUsers(IRepository repository)
        {
            var users = repository.GetAllUsers();
            if (users.Count == 0)
            {
                Console.WriteLine("No users stored.");
                return;
            }

            for (var i = 0; i < users.Count; i++)
            {
                Console.WriteLine($"User {i} : {users[i].GetShortInfo()}");
            }
        }

        private static void DisplayNotificationByIndex(IRepository repository)
        {
            Console.WriteLine("Enter notification index:");
            var input = Console.ReadLine();
            if (!int.TryParse(input, out var index))
            {
                Console.WriteLine("Invalid index.");
                return;
            }

            var item = repository.GetByIndex(index);
            if (item == null)
            {
                Console.WriteLine("Notification not found.");
                return;
            }

            Console.WriteLine(item.GetAuditLine());
        }

        private static void UpdateNotificationFlow(NotificationService service)
        {
            Console.WriteLine("Enter notification index:");
            var input = Console.ReadLine();
            if (!int.TryParse(input, out var index))
            {
                Console.WriteLine("Invalid index.");
                return;
            }

            Console.WriteLine("Enter updated message:");
            var message = Console.ReadLine() ?? string.Empty;

            try
            {
                if (service.UpdateNotification(index, message))
                {
                    Console.WriteLine("Notification updated.");
                }
            }
            catch (NotificationValidationException ex)
            {
                Console.WriteLine($"Validation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update failed: {ex.Message}");
            }
        }

        private static void DeleteNotification(IRepository repository)
        {
            Console.WriteLine("Enter notification index:");
            var input = Console.ReadLine();
            if (!int.TryParse(input, out var index))
            {
                Console.WriteLine("Invalid index.");
                return;
            }

            repository.Delete(index);
            Console.WriteLine("Delete operation completed.");
        }

        private static User ReadUser(string type)
        {
            var user = new User();

            while (true)
            {
                Console.WriteLine("Enter your name:");
                user.Name = Console.ReadLine() ?? string.Empty;

                if (!string.IsNullOrWhiteSpace(user.Name))
                {
                    break;
                }

                Console.WriteLine("Name is required.");
            }

            if (type == "email")
            {
                while (true)
                {
                    Console.WriteLine("Enter your email:");
                    user.Email = Console.ReadLine() ?? string.Empty;

                    if (user.HasValidEmail())
                    {
                        break;
                    }

                    Console.WriteLine("Invalid email address. Enter a valid email.");
                }

                Console.WriteLine("Enter your phone number (optional):");
                user.Phone = Console.ReadLine() ?? string.Empty;
            }
            else
            {
                Console.WriteLine("Enter your email (optional):");
                user.Email = Console.ReadLine() ?? string.Empty;

                while (true)
                {
                    Console.WriteLine("Enter your phone number:");
                    user.Phone = Console.ReadLine() ?? string.Empty;

                    if (user.HasValidPhone())
                    {
                        break;
                    }

                    Console.WriteLine("Invalid phone number. Enter a 10 digit phone number.");
                }
            }

            return user;
        }

        private static Notification ReadNotification()
        {
            var notification = new Notification();

            Console.WriteLine("Enter notification message:");
            notification.Message = Console.ReadLine() ?? string.Empty;

            return notification;
        }

        private static string ReadNotificationType()
        {
            Console.WriteLine("Choose notification type:");
            Console.WriteLine("1. Email");
            Console.WriteLine("2. SMS");

            while (true)
            {
                var input = Console.ReadLine();
                if (input == "1")
                {
                    return "email";
                }

                if (input == "2")
                {
                    return "sms";
                }

                Console.WriteLine("Enter 1 or 2 only:");
            }
        }
    }
}