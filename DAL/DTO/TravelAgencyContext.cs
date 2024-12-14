using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DAL.DTO
{
    public partial class TravelAgencyContext : DbContext
    {
        public TravelAgencyContext()
            : base("name=TravelAgencyDb")
        {
        }

        public virtual DbSet<bookings> bookings { get; set; }
        public virtual DbSet<images> images { get; set; }
        public virtual DbSet<reviews> reviews { get; set; }
        public virtual DbSet<tours> tours { get; set; }
        public virtual DbSet<transport> transport { get; set; }
        public virtual DbSet<users> users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<bookings>()
                .Property(e => e.totalprice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tours>()
                .Property(e => e.price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tours>()
                .HasMany(e => e.bookings)
                .WithOptional(e => e.tours)
                .WillCascadeOnDelete();

            modelBuilder.Entity<tours>()
                .HasMany(e => e.images)
                .WithOptional(e => e.tours)
                .WillCascadeOnDelete();

            modelBuilder.Entity<tours>()
                .HasMany(e => e.reviews)
                .WithOptional(e => e.tours)
                .WillCascadeOnDelete();

            modelBuilder.Entity<transport>()
                .Property(e => e.price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<users>()
                .HasMany(e => e.bookings)
                .WithOptional(e => e.users)
                .WillCascadeOnDelete();
        }
    }
}
