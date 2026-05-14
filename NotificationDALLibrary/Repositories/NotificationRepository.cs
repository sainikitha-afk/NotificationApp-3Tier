using Microsoft.EntityFrameworkCore;
using NotificationModelLibrary;

namespace NotificationDALLibrary.Repositories
{
    public class NotificationRepository
        : AbstractRepository<int, Notification>
    {
        public override Notification Add(Notification item)
        {
            context.Notifications.Add(item);
            context.SaveChanges();

            return item;
        }

        public override Notification Delete(int key)
        {
            var notification = Get(key);

            if (notification != null)
            {
                context.Notifications.Remove(notification);
                context.SaveChanges();

                return notification;
            }

            throw new Exception("Notification not found");
        }

        public override Notification? Get(int key)
        {
            return context.Notifications
                          .Include(n => n.Sender)
                          .FirstOrDefault(n => n.NotId == key);
        }

        public override ICollection<Notification> GetAll()
        {
            return context.Notifications
                          .Include(n => n.Sender)
                          .ToList();
        }

        public override Notification Update(int key, Notification item)
        {
            context.Notifications.Update(item);

            context.SaveChanges();

            return item;
        }
    }
}