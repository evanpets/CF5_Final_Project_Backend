using Microsoft.EntityFrameworkCore;

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
        public virtual DbSet<EventSave> EventSaves { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity => 
            {
                entity.ToTable("USERS");
                entity.HasIndex(e => e.Username, "UQ_USERNAME").IsUnique();
                entity.HasIndex(e => e.Email, "UQ_EMAIL").IsUnique();
                entity.Property(e => e.UserId).HasColumnName("USER_ID");
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

                entity.HasMany(u => u.Events)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("EVENTS");
                entity.Property(e => e.EventId).HasColumnName("EVENT_ID");
                entity.Property(e => e.Title)
                    .HasMaxLength(100).HasColumnName("TITLE");
                entity.Property(e => e.Description)
                    .HasMaxLength(500).HasColumnName("DESCRIPTION");
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
                entity.Property(e => e.ImageUrl)
                .HasColumnName("IMAGE_URL")
                .HasMaxLength(250);

                entity.HasOne(e => e.Venue).WithMany(v => v.Events)
                    .HasForeignKey(e => e.VenueId).HasConstraintName("FK_VENUE_EVENTS")
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasMany(e => e.Performers).WithMany(p => p.Events)
                    .UsingEntity("EVENTS_PERFORMERS");

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Events)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

            });

            modelBuilder.Entity<Venue>(entity =>
            {
                entity.ToTable("VENUES");
                entity.Property(e => e.VenueId).HasColumnName("VENUE_ID");
                entity.Property(e => e.Name)
                    .HasMaxLength(50).HasColumnName("NAME");

                entity.HasOne(v => v.VenueAddress).WithOne(va => va.Venue)
                .HasForeignKey<Venue>(v => v.VenueAddressId).HasConstraintName("FK_VENUE_ADDRESS")
                .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<Performer>(entity =>
            {
                entity.ToTable("PERFORMERS");
                entity.HasIndex(e => e.Name, "IX_PERFORMER");
                entity.Property(e => e.PerformerId).HasColumnName("PERFORMER_ID");
                entity.Property(e => e.Name)
                    .HasMaxLength(50).HasColumnName("NAME");

            });

            modelBuilder.Entity<VenueAddress>(entity =>
            {
                entity.ToTable("VENUE_ADDRESSES");
                entity.Property(e => e.VenueAddressId).HasColumnName("V_ADDRESS_ID");
                entity.Property(e => e.Street)
                    .HasMaxLength(50).HasColumnName("STREET");
                entity.Property(e => e.StreetNumber)
                    .HasMaxLength(10).HasColumnName("STREET_NUMBER");
                entity.Property(e => e.ZipCode)
                    .HasMaxLength(5).HasColumnName("ZIP_CODE");
                entity.Property(e => e.City)
                    .HasColumnName("CITY");
            });

            modelBuilder.Entity<EventSave>(entity =>
            {
                entity.ToTable("EVENT_SAVES");
                entity.HasKey(l => new { l.UserId, l.EventId }).HasName("SAVE_ID");

                entity.HasOne(e => e.User)
                    .WithMany(u => u.EventSaves)
                    .HasForeignKey(l => l.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Event)
                    .WithMany(e => e.EventSaves)
                    .HasForeignKey(e => e.EventId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
