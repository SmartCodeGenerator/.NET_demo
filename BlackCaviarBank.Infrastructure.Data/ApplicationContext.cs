﻿using BlackCaviarBank.Domain.Core;
using BlackCaviarBank.Infrastructure.Data.EntityConfigurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlackCaviarBank.Infrastructure.Data
{
    public class ApplicationContext : IdentityDbContext<UserProfile>
    {
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Card> Cards { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new AccountConfiguration());
            builder.ApplyConfiguration(new CardConfiguration());
            builder.ApplyConfiguration(new NotificationConfiguration());
            builder.ApplyConfiguration(new ServiceConfiguration());
            builder.ApplyConfiguration(new SubscriptionSubscriberConfiguration());
            builder.ApplyConfiguration(new TransactionConfiguration());
            builder.ApplyConfiguration(new UserProfileConfiguration());
        }
    }
}
