namespace NotificationApp.Models
{
    // represents a user who can receive notifications.
    // contains basic user details like name, email, and phone.
    internal class User
    {
        // unique id for user
        public int UserId { get; set; }

        // gets or sets the user's name.
        public string Name { get; set; } = string.Empty;

        // gets or sets the user's email address.
        public string Email { get; set; } = string.Empty;

        // gets or sets the user's phone number.
        public string Phone { get; set; } = string.Empty;

        // checks if the email is valid.
        // valid email must not be empty and contain '@' and '.'
        public bool HasValidEmail()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                return false;
            }

            return Email.Contains("@") &&
                   Email.Contains(".");
        }

        // checks if the phone number is valid.
        // must be exactly 10 digits.
        public bool HasValidPhone()
        {
            var digits =
                Phone.Replace(" ", "").Trim();

            return digits.Length == 10 &&
                   digits.All(char.IsDigit);
        }

        // gets short info string
        public string GetShortInfo()
        {
            return $"{Name} | {Email} | {Phone}";
        }
    }
}