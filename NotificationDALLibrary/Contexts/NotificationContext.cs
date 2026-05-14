using Microsoft.EntityFrameworkCore;
using NotificationModelLibrary;

namespace NotificationDALLibrary.Contexts
{
    public class NotificationContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=NotificationEfDb;Username=postgres;Password=postgre"
            );
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(u =>
            {
                u.HasKey(u => u.UserId);

                u.Property(u => u.Name)
                 .IsRequired()
                 .HasMaxLength(100);

                u.Property(u => u.Email)
                 .HasMaxLength(150);

                u.Property(u => u.Phone)
                 .HasMaxLength(20);
            });

            modelBuilder.Entity<Notification>(n =>
            {
                n.HasKey(n => n.NotId);

                n.Property(n => n.Message)
                 .IsRequired();

                n.Property(n => n.NotType)
                 .HasMaxLength(20);

                n.Property(n => n.SentDate)
                 .HasColumnType("timestamp without time zone");

                n.HasOne(n => n.Sender)
                 .WithMany(u => u.Notifications)
                 .HasForeignKey(n => n.UserId)
                 .HasConstraintName("FK_Notification_User")
                 .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}