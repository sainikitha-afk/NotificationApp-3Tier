using NotificationApp.Models;

namespace NotificationApp.Interfaces
{
    // basic contract for notification storage
    internal interface IRepository
    {
        // just adds a new notification to the list
        void Add(Notification notification);

        // returns everything 
        List<Notification> GetAll();

        // trying to fetch using index 
        Notification? GetByIndex(int index);

        // replace existing item
        void Update(int index, Notification notification);

        // remove based on index
        void Delete(int index);
    }
}