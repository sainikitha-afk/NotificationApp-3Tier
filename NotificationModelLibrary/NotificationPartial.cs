namespace NotificationModelLibrary
{
    public partial class Notification
    {
        public string GetAuditLine()
        {
            return $"[{SentDate:dd-MM-yyyy hh:mm tt}] {Sender?.GetShortInfo()} | {Message}";
        }
        public override string ToString()
        {
            return $"{NotId} | {NotType} | {Message}";
        }
    }
}