using NotificationApp.Interfaces;
using NotificationApp.Models;

namespace NotificationApp.Repositories
{
    internal class NotificationRepository : IRepository
    {
        // generic list data structure
        private List<Notification> notifications = new List<Notification>();

        // add
        public void Add(Notification notification)
        {
            // simple add
            notifications.Add(notification);
        }

        // get all
        public List<Notification> GetAll()
        {
            // returning full list 
            return notifications;
        }

        // get by specific id
        public Notification? GetByIndex(int index)
        {
            // checking bounds manually (just to avoid crash)
            if (index >= 0 && index < notifications.Count)
            {
                return notifications[index];
            }

            return null;
        }

        // update operation
        public void Update(int index, Notification notification)
        {
            //invalid index test case
            if (index >= 0 && index < notifications.Count)
            {
                notifications[index] = notification;
            }
            else
            {
                //will handle later
            }
        }

        public void Delete(int index)
        {
            // remove only if valid index
            if (index >= 0 && index < notifications.Count)
            {
                notifications.RemoveAt(index);
            }
        }
    }
}