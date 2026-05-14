using System.Collections.Generic;
using System.Linq;

namespace NotificationModelLibrary
{
    public partial class User
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        // navigation property - one to many relationship with Notification -> fluentapi is used to configure this in the DbContext class
        public ICollection<Notification>? Notifications { get; set; }
        public bool HasValidEmail()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                return false;
            }

            return Email.Contains("@") &&
                   Email.Contains(".");
        }
        public bool HasValidPhone()
        {
            var digits =
                Phone.Replace(" ", "").Trim();

            return digits.Length == 10 &&
                   digits.All(char.IsDigit);
        }
        public string GetShortInfo()
        {
            return $"{Name} | {Email} | {Phone}";
        }
    }
}