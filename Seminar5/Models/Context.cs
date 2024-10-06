using System;
using Seminar5.Models;
using Microsoft.EntityFrameworkCore;

namespace Seminar5.Models
{
	public class Context : DbContext
	{
		public Context()
		{
		}

		public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Messages> Massages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine).UseLazyLoadingProxies()
                .UseNpgsql("Host =localhost; Username=Maxim; Password = 1111;Database = ChatDB");
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Messages>(entity =>
            {
                entity.HasKey(x => x.Id).HasName("massagePK");
                entity.ToTable("Massages"); 
                entity.Property(x => x.Id).HasColumnName("id");
                entity.Property(x => x.Text).HasColumnName("text");
                entity.Property(x => x.ToUserId).HasColumnName("to_user_id");
                entity.Property(x => x.FromUserId).HasColumnName("from_user_id");
                entity.HasOne(d => d.FromUser).WithMany(p => p.FromMassages)
                .HasForeignKey(e => e.FromUserId).HasConstraintName("massages_from_user_if_fkey");
                entity.HasOne(d => d.ToUser).WithMany(p => p.ToMassages)
                .HasForeignKey(e => e.ToUserId).HasConstraintName("massages_to_user_if_fkey");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(x => x.Id).HasName("user_pkey");
                entity.ToTable("Users");
                entity.Property(x => x.Id).HasColumnName("id");
                entity.Property(x => x.Name).HasMaxLength(255).HasColumnName("name");

            });
            base.OnModelCreating(modelBuilder);
        }  
    }
}

