namespace NotificationApp.Models
{
    // represents a user who can receive notifications.
    // contains basic user details like name, email, and phone.
    internal class User
    {
        // gets or sets the user's name.
        public string Name { get; set; } = string.Empty;

        // gets or sets the user's email address.
        public string Email { get; set; } = string.Empty;

        // gets or sets the user's phone number.
        public string Phone { get; set; } = string.Empty;


        // checks if the email is valid.
        // valid email must not be empty and contain '@' and '.'.
        public bool HasValidEmail()
        {
            // check if email is empty or whitespace
            if (string.IsNullOrWhiteSpace(Email))
            {
                return false;
            }
            return Email.Contains("@") && Email.Contains(".");
        }

        // checks if the phone number is valid.
        // must be exactly 10 digits.
        public bool HasValidPhone()
        {
            // remove spaces and trim
            var digits = Phone.Replace(" ", "").Trim();

            // must be 10 digits and all characters are digits
            return digits.Length == 10 && digits.All(char.IsDigit);
        }   

        // gets a short info string with name, email, and phone.
        public string GetShortInfo()
        {
            return $"{Name} | {Email} | {Phone}";
        }
    }
}
