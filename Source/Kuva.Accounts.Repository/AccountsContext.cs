using System;
using Microsoft.EntityFrameworkCore;
using Kuva.Accounts.Repository.Domain;
using Kuva.Accounts.Repository.Data.Interfaces;

namespace Kuva.Accounts.Repository
{
    public class AccountsContext: DbContext, IAccountsContext
    {
        public AccountsContext(DbContextOptions<AccountsContext> options) : base(options) { }
        
        public DbSet<UserDomain> User { get; }
        public DbSet<UserLevelDomain> UserLevel { get; }
        public DbSet<UserTokenDomain> UserToken { get; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<UserLevelDomain>(entity =>
            {
                entity.ToTable("UserLevel", Constants.ServiceSchemaName);
                entity.HasKey(_ => _.Id);
                entity.Property(_ => _.Id).ValueGeneratedOnAdd();
                entity.HasIndex(_ => _.Name).IsUnique();
                entity.Property(_ => _.Name).HasMaxLength(60).IsRequired();
                entity.Property(_ => _.Active).IsRequired();
                entity.Property(_ => _.CreateAt).IsRequired();
                
                var administratorLevel = new UserLevelDomain()
                    { Id = 1, Active = true, Name = "Administrador", CreateAt = DateTime.Now};
                var userLevel = new UserLevelDomain()
                    { Id = 2, Active = true, Name = "User", CreateAt = DateTime.Now};
                var clientLevel = new UserLevelDomain()
                    { Id = 3, Active = true, Name = "Client", CreateAt = DateTime.Now};
                var guestLevel = new UserLevelDomain()
                    { Id = 4, Active = true, Name = "Guest", CreateAt = DateTime.Now};
                
                entity.HasData(administratorLevel, userLevel, clientLevel, guestLevel);
            });

            modelBuilder.Entity<UserDomain>(entity =>
            {
                entity.ToTable("User", Constants.ServiceSchemaName);
                entity.HasKey(_ => _.Id);
                entity.Property(_ => _.Id).ValueGeneratedOnAdd();
                entity.HasIndex(_ => _.Email).IsUnique();
                entity.Property(_ => _.UserLevelId).IsRequired();
                entity.Property(_ => _.Active).IsRequired();
                entity.Property(_ => _.CreateAt).IsRequired();
                entity.Property(_ => _.Email).HasMaxLength(180).IsRequired();
                entity.Property(_ => _.Name).HasMaxLength(60).IsRequired();
                entity.Property(_ => _.Passcode).HasMaxLength(80).IsRequired(); 
            });

            modelBuilder.Entity<UserTokenDomain>(entity =>
            {
                entity.ToTable("UserToken", Constants.ServiceSchemaName);
                entity.HasKey(_ => _.Id);
                entity.Property(_ => _.Id).IsRequired();
                entity.HasIndex(_ => _.UserId).IsUnique();
                entity.Property(_ => _.ConfirmationCode).IsRequired();
                entity.Property(_ => _.ConfirmationCode).HasMaxLength(40).IsRequired();
                entity.Property(_ => _.CreateAt).IsRequired(); 
            });
        }
    }
}