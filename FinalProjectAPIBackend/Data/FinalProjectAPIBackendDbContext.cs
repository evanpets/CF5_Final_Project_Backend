﻿using Microsoft.EntityFrameworkCore;

namespace FinalProjectAPIBackend.Data
{
    public class FinalProjectAPIBackendDbContext : DbContext
    {
        public FinalProjectAPIBackendDbContext()
        {
        }

        public FinalProjectAPIBackendDbContext(DbContextOptions<FinalProjectAPIBackendDbContext> options) : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Performer> Performers { get; set; }
        public virtual DbSet<Venue> Venues { get; set; }
        public virtual DbSet<VenueAddress> VenueAddresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity => 
            {
                entity.ToTable("USERS");
                entity.HasIndex(e => e.Username, "UQ_USERNAME").IsUnique();
                entity.HasIndex(e => e.Email, "UQ_EMAIL").IsUnique();
                entity.Property(e => e.UserId).HasColumnName("ID");
                entity.Property(e => e.Username)
                    .HasMaxLength(50).HasColumnName("USERNAME");
                entity.Property(e => e.Password)
                    .HasMaxLength(512).HasColumnName("PASSWORD");
                entity.Property(e => e.Email)
                    .HasMaxLength(50).HasColumnName("EMAIL");
                entity.Property(e => e.FirstName)
                    .HasMaxLength(50).HasColumnName("FIRST_NAME");
                entity.Property(e => e.LastName)
                    .HasMaxLength(50).HasColumnName("LAST_NAME");
                entity.Property(e => e.PhoneNumber).HasMaxLength(13).HasColumnName("PHONE_NUMBER");
                entity.Property(e => e.Role)
                    .HasColumnName("USER_ROLE")
                    .HasConversion<string>()
                    .HasMaxLength(50)
                    .IsRequired();
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("EVENTS");
                entity.Property(e => e.EventId).HasColumnName("ID");
                entity.Property(e => e.Title)
                    .HasMaxLength(50).HasColumnName("TITLE");
                entity.Property(e => e.Description)
                    .HasMaxLength(250).HasColumnName("DESCRIPTION");
                //entity.HasOne(e => e.Venue).WithOne(v => v.Event)
                //.HasForeignKey<Venue>(e => e.Id).HasConstraintName("FK_VENUES_EVENTS")
                //.OnDelete(DeleteBehavior.SetNull);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)")
                    .HasColumnName("PRICE");
                entity.Property(e => e.Date).HasColumnName("EVENT_DATE")
                .HasConversion
                    (d => d.HasValue ? d.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                    d => d.HasValue ? DateOnly.FromDateTime(d.Value) : (DateOnly?)null);
            entity.Property(e => e.Category)
                    .HasColumnName("CATEGORY")
                    .HasConversion<string>()
                    .HasMaxLength(50)
                    .IsRequired();

                entity.HasOne(e => e.Venue).WithMany(v => v.Events)
                    .HasForeignKey(e => e.VenueId).HasConstraintName("FK_VENUE_EVENTS")
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasMany(e => e.Performers).WithMany(p => p.Events)
                    .UsingEntity("EVENTS_PERFORMERS");

            });

            modelBuilder.Entity<Venue>(entity =>
            {
                entity.ToTable("VENUES");
                entity.Property(e => e.VenueId).HasColumnName("ID");
                entity.Property(e => e.Name)
                    .HasMaxLength(50).HasColumnName("NAME");
                entity.HasOne(v => v.VenueAddress).WithOne(va => va.Venue)
                .HasForeignKey<VenueAddress>(v => v.VenueAddressId).HasConstraintName("FK_VENUE_ADDRESS")
                .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<Performer>(entity =>
            {
                entity.ToTable("PERFORMERS");
                entity.HasIndex(e => e.Name, "IX_PERFORMER");
                entity.Property(e => e.PerformerId).HasColumnName("ID");
                entity.Property(e => e.Name)
                    .HasMaxLength(50).HasColumnName("NAME");
                //entity.HasMany(p => p.Events).WithMany(e => e.Performers)
                //.UsingEntity("EVENTS_PERFORMERS");

            });

            modelBuilder.Entity<VenueAddress>(entity =>
            {
                entity.ToTable("VENUE_ADDRESSES");
                entity.Property(e => e.VenueAddressId).HasColumnName("ID");
                entity.Property(e => e.Street)
                    .HasMaxLength(50).HasColumnName("STREET");
                entity.Property(e => e.StreetNumber)
                    .HasMaxLength(10).HasColumnName("STREET_NUMBER");
                entity.Property(e => e.ZipCode)
                    .HasMaxLength(5).HasColumnName("ZIP_CODE");
            });
        }
    }
}