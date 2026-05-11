using System;
using System.Collections.Generic;
using NotificationApp.Interfaces;
using NotificationApp.Models;
using Npgsql;

namespace NotificationApp.DataAccessLayer
{
    internal class NotificationRepository : IRepository
    {
        // postgre sql connection string
        private readonly string connectionString =
            "Host=localhost;Port=5432;Database=NotificationDB;Username=postgres;Password=postgre";

        // connection object
        NpgsqlConnection connection;

        // constructor
        public NotificationRepository()
        {
            connection = new NpgsqlConnection(connectionString);
        }

        // adds a user into database
        public void AddUser(User user)
        {
            string insQ =
                $"INSERT INTO users(name, email, phone) " +
                $"VALUES('{user.Name}', '{user.Email}', '{user.Phone}')";

            NpgsqlCommand command =
                new NpgsqlCommand(insQ, connection);

            try
            {
                connection.Open();

                int result =
                    command.ExecuteNonQuery();

                if (result > 0)
                {
                    Console.WriteLine("User added to database");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection?.Close();
            }
        }

        // gets a user by email or phone
        public User? GetUser(string email, string phone)
        {
            string selQ =
                $"SELECT * FROM users " +
                $"WHERE email='{email}' OR phone='{phone}'";

            NpgsqlCommand command =
                new NpgsqlCommand(selQ, connection);

            try
            {
                connection.Open();

                NpgsqlDataReader reader =
                    command.ExecuteReader();

                if (reader.Read())
                {
                    User user = new User();

                    user.UserId =
                        Convert.ToInt32(reader["user_id"]);

                    user.Name =
                        reader["name"].ToString() ?? "";

                    user.Email =
                        reader["email"].ToString() ?? "";

                    user.Phone =
                        reader["phone"].ToString() ?? "";

                    return user;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection?.Close();
            }

            return null;
        }

        // adds notification into database
        public void Add(Notification note)
        {
            string sentDate =
                note.SentDate.ToString("yyyy-MM-dd HH:mm:ss");

            string insQ =
                $"INSERT INTO notifications(message, sent_date, notification_type, user_id) " +
                $"VALUES('{note.Message}', '{sentDate}', '{note.NotType}', {note.Sender.UserId})";

            NpgsqlCommand command =
                new NpgsqlCommand(insQ, connection);

            try
            {
                connection.Open();

                int result =
                    command.ExecuteNonQuery();

                if (result > 0)
                {
                    Console.WriteLine("Notification stored in database");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection?.Close();
            }
        }

        // gets all notifications
        public List<Notification> GetAll()
        {
            List<Notification> notifications =
                new List<Notification>();

            string selQ =
                @"SELECT n.notification_id,
                         n.message,
                         n.sent_date,
                         n.notification_type,
                         u.user_id,
                         u.name,
                         u.email,
                         u.phone
                  FROM notifications n
                  JOIN users u
                  ON n.user_id = u.user_id";

            NpgsqlCommand command =
                new NpgsqlCommand(selQ, connection);

            try
            {
                connection.Open();

                NpgsqlDataReader reader =
                    command.ExecuteReader();

                while (reader.Read())
                {
                    User user = new User();

                    user.UserId =
                        Convert.ToInt32(reader["user_id"]);

                    user.Name =
                        reader["name"].ToString() ?? "";

                    user.Email =
                        reader["email"].ToString() ?? "";

                    user.Phone =
                        reader["phone"].ToString() ?? "";

                    Notification note =
                        new Notification();

                    note.NotId =
                        Convert.ToInt32(reader["notification_id"]);

                    note.Message =
                        reader["message"].ToString() ?? "";

                    note.SentDate =
                        Convert.ToDateTime(reader["sent_date"]);

                    note.NotType =
                        reader["notification_type"].ToString() ?? "";

                    note.Sender = user;

                    notifications.Add(note);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection?.Close();
            }

            return notifications;
        }

        // gets notification by id
        public Notification? GetByIndex(int idx)
        {
            string selQ =
                @"SELECT n.notification_id,
                         n.message,
                         n.sent_date,
                         n.notification_type,
                         u.user_id,
                         u.name,
                         u.email,
                         u.phone
                  FROM notifications n
                  JOIN users u
                  ON n.user_id = u.user_id
                  WHERE n.notification_id = " + idx;

            NpgsqlCommand command =
                new NpgsqlCommand(selQ, connection);

            try
            {
                connection.Open();

                NpgsqlDataReader reader =
                    command.ExecuteReader();

                if (reader.Read())
                {
                    User user = new User();

                    user.UserId =
                        Convert.ToInt32(reader["user_id"]);

                    user.Name =
                        reader["name"].ToString() ?? "";

                    user.Email =
                        reader["email"].ToString() ?? "";

                    user.Phone =
                        reader["phone"].ToString() ?? "";

                    Notification note =
                        new Notification();

                    note.NotId =
                        Convert.ToInt32(reader["notification_id"]);

                    note.Message =
                        reader["message"].ToString() ?? "";

                    note.SentDate =
                        Convert.ToDateTime(reader["sent_date"]);

                    note.NotType =
                        reader["notification_type"].ToString() ?? "";

                    note.Sender = user;

                    return note;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection?.Close();
            }

            return null;
        }

        // updates notification
        public void Update(int idx, Notification note)
        {
            string sentDate =
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string updQ =
                $"UPDATE notifications " +
                $"SET message='{note.Message}', " +
                $"sent_date='{sentDate}' " +
                $"WHERE notification_id={idx}";

            NpgsqlCommand command =
                new NpgsqlCommand(updQ, connection);

            try
            {
                connection.Open();

                int result =
                    command.ExecuteNonQuery();

                if (result > 0)
                {
                    Console.WriteLine("Notification updated successfully");
                }
                else
                {
                    Console.WriteLine("Notification not found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection?.Close();
            }
        }

        // deletes notification
        public void Delete(int idx)
        {
            string delQ =
                $"DELETE FROM notifications " +
                $"WHERE notification_id={idx}";

            NpgsqlCommand command =
                new NpgsqlCommand(delQ, connection);

            try
            {
                connection.Open();

                int result =
                    command.ExecuteNonQuery();

                if (result > 0)
                {
                    Console.WriteLine("Notification deleted successfully");
                }
                else
                {
                    Console.WriteLine("Notification not found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection?.Close();
            }
        }

        // gets all users
        public List<User> GetAllUsers()
        {
            List<User> users =
                new List<User>();

            string selQ =
                "SELECT * FROM users";

            NpgsqlCommand command =
                new NpgsqlCommand(selQ, connection);

            try
            {
                connection.Open();

                NpgsqlDataReader reader =
                    command.ExecuteReader();

                while (reader.Read())
                {
                    User user = new User();

                    user.UserId =
                        Convert.ToInt32(reader["user_id"]);

                    user.Name =
                        reader["name"].ToString() ?? "";

                    user.Email =
                        reader["email"].ToString() ?? "";

                    user.Phone =
                        reader["phone"].ToString() ?? "";

                    users.Add(user);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection?.Close();
            }

            return users;
        }
    }
}