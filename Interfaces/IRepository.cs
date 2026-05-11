using NotificationApp.Models;

namespace NotificationApp.Interfaces
{
    // repository that manages notifications
    // CRUD operations for notification storage
    internal interface IRepository
    {
        // adds a new notification to the repository.
        public void Add(Notification note);

        // retrieves all notifications from the repository.
        public List<Notification> GetAll();

        // retrieves a notification by its index.
        public Notification? GetByIndex(int idx); // if index is not found, should return null

        // updates an existing notification at the specified index.
        public void Update(int idx, Notification note);

        // deletes a notification at the specified index.
        public void Delete(int idx);

        // adds a new user to the repository.
        public void AddUser(User user);

        // retrieves all users from the repository.
        public List<User> GetAllUsers();

        // retrieves a user by contact details.
        public User? GetUser(string email, string phone);
    }
}
