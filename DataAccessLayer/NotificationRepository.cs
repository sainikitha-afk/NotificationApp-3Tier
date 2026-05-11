using System;
using System.Linq;
using NotificationApp.Interfaces;
using NotificationApp.Models;

namespace NotificationApp.DataAccessLayer
{
    internal class NotificationRepository : IRepository
    {
        private readonly List<Notification> _sent = new();
        private readonly List<User> _users = new();

        public void Add(Notification note)
        {
            ArgumentNullException.ThrowIfNull(note);
            _sent.Add(note);
        }

        public List<Notification> GetAll()
        {
            return new List<Notification>(_sent);
        }

        public Notification? GetByIndex(int idx)
        {
            return idx >= 0 && idx < _sent.Count ? _sent[idx] : null;
        }

        public void Update(int idx, Notification note)
        {
            if (note == null || idx < 0 || idx >= _sent.Count)
            {
                return;
            }

            _sent[idx] = note;
        }

        public void Delete(int idx)
        {
            if (idx < 0 || idx >= _sent.Count)
            {
                return;
            }

            _sent.RemoveAt(idx);
        }

        public void AddUser(User user)
        {
            ArgumentNullException.ThrowIfNull(user);
            _users.Add(user);
        }

        public List<User> GetAllUsers()
        {
            return new List<User>(_users);
        }

        public User? GetUser(string email, string phone)
        {
            var normalizedEmail = email?.Trim().ToLowerInvariant() ?? string.Empty;
            var normalizedPhone = phone?.Replace(" ", "").Trim() ?? string.Empty;

            return _users.FirstOrDefault(user =>
                (!string.IsNullOrEmpty(normalizedEmail) &&
                    user.Email.Trim().ToLowerInvariant() == normalizedEmail)
                || (!string.IsNullOrEmpty(normalizedPhone) &&
                    user.Phone.Replace(" ", "").Trim() == normalizedPhone));
        }
    }
}
