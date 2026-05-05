using NotificationApp.Models;
using NotificationApp.Services;
using NotificationApp.Interfaces;
using NotificationApp.Repositories;

namespace NotificationApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool flag = true;

            IRepository repo = new NotificationRepository();

            while (flag)
            {
                Console.WriteLine("\n menu");
                Console.WriteLine("1. create notificaiton");
                Console.WriteLine("2. view all");
                Console.WriteLine("3. view by specific index");
                Console.WriteLine("4. update");
                Console.WriteLine("5. delete");
                Console.WriteLine("6. exit");

                int ch;

                //forcing valid input 
                while (!int.TryParse(Console.ReadLine(), out ch))
                {
                    Console.WriteLine("enter a valid choice:");
                }

                switch (ch) 
                {
                    case 1 :
                        //create
                        // creating user obj
                        User user = new User();

                        Console.WriteLine("enter your name:");
                        user.Name = Console.ReadLine() ?? "";
                        while (string.IsNullOrWhiteSpace(user.Name))
                        {
                            Console.WriteLine("invalid name, enter again.");
                            user.Name = Console.ReadLine() ?? "";
                        }

                        Console.WriteLine("enter your email:");
                        user.Email = Console.ReadLine() ?? "";
                        while (string.IsNullOrWhiteSpace(user.Email))
                        {
                            Console.WriteLine("invalid email, enter again;");
                            user.Email = Console.ReadLine() ?? "";
                        }

                        Console.WriteLine("enter your phone number:");
                        user.Phone = Console.ReadLine() ?? "";
                        while (string.IsNullOrWhiteSpace(user.Phone) || user.Phone.Length != 10 || !user.Phone.All(char.IsDigit))
                        {
                            Console.WriteLine("invalid ph no. enter again:");
                            user.Phone = Console.ReadLine() ?? "";
                        }

                        // create notification object
                        Notification notification = new Notification();

                        Console.WriteLine("enter message");
                        notification.Message = Console.ReadLine() ?? "";
                        while (string.IsNullOrWhiteSpace(notification.Message))
                        {
                            Console.WriteLine("invalid message");
                            notification.Message = Console.ReadLine() ?? "";
                        }

                        notification.SentDate = DateTime.Now;

                        // to ask user how they want to receive notification
                        Console.WriteLine("\n chooose your notification type:");
                        Console.WriteLine("1. email");
                        Console.WriteLine("2. SMS");

                        int choice;
                        while (!int.TryParse(Console.ReadLine(), out choice) || (choice != 1 && choice != 2))
                        {
                            Console.WriteLine("invalid choice, please enter 1 or 2 only.");
                        }

                        // create service
                        NotificationService service = new NotificationService();

                        // ref of interface type (polymorphism)
                        INotification notifier;

                        // choice which notification to use
                        if (choice == 1) {
                            notifier = new EmailNotification();
                        }
                        else if (choice == 2) {
                            notifier = new SMSNotification();
                        }
                        else {

                            Console.WriteLine("invalid choice");
                            return;
                        }

                        // send notification
                        service.SendNotification(notifier, user, notification);

                        Console.WriteLine("\nnotification sent successfully!");
                        
                        repo.Add(notification);

                        Console.WriteLine("\ndo you want to send another notification or check out the ? (y/n)");
                        string again = Console.ReadLine() ?? "";

                        if (again.ToLower() != "y")
                        {
                            continue;
                        }
                        //storing

                        break;
                    
                    case 2:
                        // read all
                        var list = repo.GetAll();

                        // looping through each item 
                        for (int i = 0; i < list.Count; i++)
                        {
                            Console.WriteLine($"index {i}: {list[i].Message}");
                        }
                        break;
                    
                    case 3:
                        // read one
                        Console.WriteLine("enter index:");
                        int idx;

                        while (!int.TryParse(Console.ReadLine(), out idx))
                        {
                            Console.WriteLine("invalid index, try again:");
                        }

                        var item = repo.GetByIndex(idx);

                        if (item != null)
                        {
                            Console.WriteLine(item.Message);
                        }
                        else
                        {
                            Console.WriteLine("not found");
                        }

                        break;
                    
                    case 4:
                        //update
                        Console.WriteLine("enter index:");
                        int uidx;

                        while (!int.TryParse(Console.ReadLine(), out uidx))
                        {
                            Console.WriteLine("enter a valid number:");
                        }

                        Console.WriteLine("enter new message:");
                        string newMsg = Console.ReadLine() ?? "";

                        // creating new object instead of modifying existing
                        Notification updated = new Notification();
                        updated.Message = newMsg;
                        updated.SentDate = DateTime.Now;

                        repo.Update(uidx, updated);

                        break;
                    
                    case 5:
                        // delete
                        Console.WriteLine("enter index:");
                        int didx;
                        while (!int.TryParse(Console.ReadLine(), out didx))
                        {
                            Console.WriteLine("invalid input, enter a number:");
                        }
                        repo.Delete(didx);
                        Console.WriteLine("valid index - deleted sucessfully");
                        break;

                    case 6:
                        // exit loop
                        flag = false;
                        break;
                    
                    default:
                        Console.WriteLine("invalid option");
                        break;
                }
            }
        }
    }
}