using System;
using NotificationApp.Interfaces;
using NotificationApp.Models;

namespace NotificationApp.DataAccessLayer
{
    internal class NotificationRepository : IRepository
    {
        private readonly List<Notification> _sent = new();

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
    }
}
