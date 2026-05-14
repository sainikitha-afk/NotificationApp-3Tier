using Microsoft.EntityFrameworkCore;
using NotificationDALLibrary.Contexts;
using NotificationDALLibrary.Interfaces;

namespace NotificationDALLibrary.Repositories
{
    public abstract class AbstractRepository<K, T> : IRepository<K, T>
        where T : class
    {
        protected readonly NotificationContext context;

        public AbstractRepository()
        {
            context = new NotificationContext();
        }

        public abstract T Add(T item);

        public abstract T Delete(K key);

        public abstract T? Get(K key);

        public abstract ICollection<T> GetAll();

        public abstract T Update(K key, T item);
    }
}