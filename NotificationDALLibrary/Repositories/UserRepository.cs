using NotificationModelLibrary;

namespace NotificationDALLibrary.Repositories
{
    public class UserRepository
        : AbstractRepository<int, User>
    {
        public override User Add(User item)
        {
            context.Users.Add(item);

            context.SaveChanges();

            return item;
        }

        public override User Delete(int key)
        {
            var user = Get(key);

            if (user != null)
            {
                context.Users.Remove(user);

                context.SaveChanges();

                return user;
            }

            throw new Exception("User not found");
        }

        public override User? Get(int key)
        {
            return context.Users
                          .FirstOrDefault(u => u.UserId == key);
        }

        public User? GetByContact(string email, string phone)
        {
            return context.Users.FirstOrDefault(
                u => u.Email == email || u.Phone == phone
            );
        }

        public override ICollection<User> GetAll()
        {
            return context.Users.ToList();
        }

        public override User Update(int key, User item)
        {
            var user = Get(key);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.Name = item.Name;
            user.Email = item.Email;
            user.Phone = item.Phone;

            context.SaveChanges();

            return user;
        }
    }
}