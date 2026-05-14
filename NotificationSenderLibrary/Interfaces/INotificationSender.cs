using NotificationModelLibrary;

namespace NotificationSenderLibrary.Interfaces
{
    public interface INotificationSender
    {
        void Send(User user, Notification notification);
    }
}