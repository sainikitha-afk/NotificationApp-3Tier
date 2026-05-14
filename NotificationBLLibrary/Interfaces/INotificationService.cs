using NotificationModelLibrary;

namespace NotificationBLLibrary.Interfaces
{
    public interface INotificationService
    {
        bool SendNotification(User user, Notification notification, string type);

        bool UpdateNotification(int id, string message);
    }
}