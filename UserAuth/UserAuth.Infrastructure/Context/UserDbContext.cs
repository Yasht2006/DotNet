using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuth.Domain.Models;

namespace UserAuth.Infrastructure.Context
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<BlacklistedToken> BlackListedTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasKey(u => u.Id);

                entity.Property(u => u.Name)
                      .IsRequired()
                      .HasMaxLength(40);

                entity.Property(u => u.Phone)
                      .IsRequired()
                      .HasMaxLength(15);

                entity.Property(u => u.Email)
                      .IsRequired()
                      .HasMaxLength(80);

                entity.HasIndex(u => u.Email)
                      .IsUnique();

                entity.Property(u => u.Password)
                      .IsRequired();

                entity.Property(u => u.Role)
                      .IsRequired()
                      .HasMaxLength(10);

                entity.Property(u => u.CreatedBy)
                      .IsRequired();

                entity.Property(u => u.CreatedOn)
                      .IsRequired();

                entity.Property(u => u.UpdatedBy);

                entity.Property(u => u.UpdatedOn);


                entity.HasOne(u => u.CreatedByUser)
                      .WithMany()
                      .HasForeignKey(u => u.CreatedBy)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(u => u.UpdatedByUser)
                      .WithMany()
                      .HasForeignKey(u => u.UpdatedBy)
                      .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<BlacklistedToken>(entity =>
            {
                entity.ToTable("blacklistedtokens");

                entity.HasKey(u => u.Id);

                entity.Property(u => u.Token);

                entity.Property(u => u.Expiration);
            });
        }
    }
}
