namespace NotificationDALLibrary.Interfaces
{
    public interface IRepository<K, T> where T : class
    {
        T Add(T item);

        T? Get(K key);

        ICollection<T> GetAll();

        T Update(K key, T item);

        T Delete(K key);
    }
}