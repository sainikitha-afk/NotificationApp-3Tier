namespace NotificationModelLibrary
{
    public partial class User
    {
        public override string ToString()
        {
            return $"{UserId} - {Name} | {Email} | {Phone}";
        }
    }
}