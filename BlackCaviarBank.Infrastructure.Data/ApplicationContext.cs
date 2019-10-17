using BlackCaviarBank.Domain.Core;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace BlackCaviarBank.Infrastructure.Data
{
    public class ApplicationContext : IdentityDbContext<UserProfile>
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Account>().Property(a => a.AccountNumber).HasMaxLength(20).IsRequired();
            builder.Entity<Account>().HasIndex(a => a.AccountNumber).IsUnique();
            builder.Entity<Account>().Property(a => a.Name).HasMaxLength(30).IsRequired();
            builder.Entity<Account>().Property(a => a.OpeningDate).HasDefaultValue(DateTime.Now).IsRequired();
            builder.Entity<Account>().Property(a => a.Balance).HasDefaultValue(0).IsRequired();
            builder.Entity<Account>().HasOne(a => a.Owner).WithMany(up => up.Accounts).HasForeignKey(a => a.OwnerId).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Card>().Property(c => c.CardNumber).HasMaxLength(16).IsRequired();
            builder.Entity<Card>().HasIndex(c => c.CardNumber).IsUnique();
            builder.Entity<Card>().Property(c => c.ExpirationDate).HasDefaultValue(DateTime.Now).IsRequired();
            builder.Entity<Card>().Property(c => c.PaymentSystem).HasDefaultValue("Visa").IsRequired();
            builder.Entity<Card>().Property(c => c.CVV2).HasMaxLength(3).IsRequired();
            builder.Entity<Card>().Property(c => c.Balance).HasDefaultValue(0).IsRequired();
            builder.Entity<Card>().HasOne(c => c.Owner).WithMany(up => up.Cards).HasForeignKey(c => c.OwnerId).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Notification>().Property(n => n.Text).HasMaxLength(150).IsRequired();
            builder.Entity<Notification>().Property(n => n.Time).HasDefaultValue(DateTime.Now).IsRequired();
            builder.Entity<Notification>().HasOne(n => n.Receiver).WithMany(up => up.Notifications).HasForeignKey(n => n.ReceiverId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Notification>().HasOne(n => n.Sender).WithMany(serv => serv.Notifications).HasForeignKey(n => n.SenderId).OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Service>().Property(serv => serv.Name).HasMaxLength(30).IsRequired();
            builder.Entity<Service>().HasIndex(serv => serv.Name).IsUnique();
            builder.Entity<Service>().Property(serv => serv.Price).HasDefaultValue(0).IsRequired();

            builder.Entity<SubscriptionSubscriber>().HasKey(ss => new { ss.SubscriberId, ss.SubscriptionId });
            builder.Entity<SubscriptionSubscriber>().HasOne(ss => ss.Subscriber).WithMany(up => up.SubscriptionSubscribers).HasForeignKey(ss => ss.SubscriberId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<SubscriptionSubscriber>().HasOne(ss => ss.Subscription).WithMany(serv => serv.SubscriptionSubscribers).HasForeignKey(ss => ss.SubscriptionId).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Transaction>().Property(t => t.From).HasMaxLength(20).IsRequired();
            builder.Entity<Transaction>().Property(t => t.To).HasMaxLength(20).IsRequired();
            builder.Entity<Transaction>().Property(t => t.Amount).HasDefaultValue(0).IsRequired();
            builder.Entity<Transaction>().Property(t => t.Date).HasDefaultValue(DateTime.Now).IsRequired();
            builder.Entity<Transaction>().HasOne(t => t.Payer).WithMany(up => up.Transactions).HasForeignKey(t => t.PayerId).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserProfile>().Property(up => up.FirstName).HasMaxLength(20).IsRequired();
            builder.Entity<UserProfile>().Property(up => up.LastName).HasMaxLength(20).IsRequired();
        }
    }
}
