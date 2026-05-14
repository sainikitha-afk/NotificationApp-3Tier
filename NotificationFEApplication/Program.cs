using NotificationBLLibrary.Exceptions;
using NotificationBLLibrary.Services;
using NotificationDALLibrary.Repositories;
using NotificationModelLibrary;

namespace NotificationFEApplication
{
    internal class Program
    {
        static readonly NotificationService service =
            new NotificationService();

        static readonly NotificationRepository notificationRepository =
            new NotificationRepository();

        static readonly UserRepository userRepository =
            new UserRepository();

        static void Main(string[] args)
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine();
                Console.WriteLine(" Notification App Menu");
                Console.WriteLine(" Input only from numbers 1 to 6 or 0 to exit the app.");
                Console.WriteLine("1. Send Notification");
                Console.WriteLine("2. View All Notifications");
                Console.WriteLine("3. View Notification By Id");
                Console.WriteLine("4. Update Notification");
                Console.WriteLine("5. Delete Notification");
                Console.WriteLine("6. View All Users");
                Console.WriteLine("0. Exit");

                Console.Write("Enter choice : ");

                int.TryParse(Console.ReadLine(), out int choice);

                try
                {
                    switch (choice)
                    {
                        case 1:
                            SendNotificationFlow();
                            break;

                        case 2:
                            ViewAllNotifications();
                            break;

                        case 3:
                            ViewNotificationById();
                            break;

                        case 4:
                            UpdateNotification();
                            break;

                        case 5:
                            DeleteNotification();
                            break;

                        case 6:
                            ViewAllUsers();
                            break;

                        case 0:
                            exit = true;
                            break;

                        default:
                            Console.WriteLine("Invalid choice");
                            break;
                    }
                }
                catch (NotificationValidationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (NotificationProcessException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        static void SendNotificationFlow()
        {
            Console.Write("Enter notification type (email/sms) : ");

            string type =
                (Console.ReadLine() ?? "")
                .Trim()
                .ToLower();

            User user = new User();

            Console.Write("Enter user name : ");

            user.Name =
                Console.ReadLine() ?? "";

            if (type == "email")
            {
                Console.Write("Enter email : ");

                user.Email =
                    Console.ReadLine() ?? "";
            }
            else if (type == "sms")
            {
                Console.Write("Enter phone number : ");

                user.Phone =
                    Console.ReadLine() ?? "";
            }
            else
            {
                Console.WriteLine(
                    "Notification type must be email or sms."
                );

                return;
            }

            Notification notification =
                new Notification();

            Console.Write("Enter message : ");

            notification.Message =
                Console.ReadLine() ?? "";

            bool result =
                service.SendNotification(
                    user,
                    notification,
                    type
                );

            if (result)
            {
                Console.WriteLine();
                Console.WriteLine(
                    "Notification sent successfully."
                );
            }
        }

        static User TakeUserDetails()
        {
            User user = new User();

            Console.Write("Enter user name : ");
            user.Name = Console.ReadLine() ?? "";

            Console.Write("Enter email : ");
            user.Email = Console.ReadLine() ?? "";

            Console.Write("Enter phone : ");
            user.Phone = Console.ReadLine() ?? "";

            return user;
        }

        static Notification TakeNotificationDetails()
        {
            Notification notification = new Notification();

            Console.Write("Enter message : ");

            notification.Message =
                Console.ReadLine() ?? "";

            return notification;
        }

        static void ViewAllNotifications()
        {
            var notifications =
                notificationRepository.GetAll();

            if (notifications.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("No notifications found.");
                return;
            }

            Console.WriteLine();

            foreach (var item in notifications)
            {
                Console.WriteLine(item.GetAuditLine());
            }
        }

        static void ViewNotificationById()
        {
            Console.Write("Enter notification id : ");

            int.TryParse(Console.ReadLine(), out int id);

            var notification =
                notificationRepository.Get(id);

            if (notification == null)
            {
                Console.WriteLine("Notification not found");
                return;
            }

            Console.WriteLine(notification.GetAuditLine());
        }

        static void UpdateNotification()
        {
            Console.Write("Enter notification id : ");

            int.TryParse(Console.ReadLine(), out int id);

            Console.Write("Enter updated message : ");

            string message =
                Console.ReadLine() ?? "";

            service.UpdateNotification(id, message);

            Console.WriteLine("Notification updated");
        }

        static void DeleteNotification()
        {
            Console.Write("Enter notification id : ");

            int.TryParse(Console.ReadLine(), out int id);

            notificationRepository.Delete(id);

            Console.WriteLine("Notification deleted");
        }

        static void ViewAllUsers()
        {
            var users = userRepository.GetAll();

            foreach (var user in users)
            {
                Console.WriteLine(user);
            }
        }
    }
}