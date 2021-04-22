using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace InpatientTherapySchedulingProgram.Models
{
    public partial class CoreDbContext : DbContext
    {
        public CoreDbContext()
        {
        }

        public CoreDbContext(DbContextOptions<CoreDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Appointment> Appointment { get; set; }
        public virtual DbSet<HoursWorked> HoursWorked { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<Patient> Patient { get; set; }
        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<Room> Room { get; set; }
        public virtual DbSet<TherapistEvent> TherapistEvent { get; set; }
        public virtual DbSet<Therapy> Therapy { get; set; }
        public virtual DbSet<TherapyMain> TherapyMain { get; set; }
        public virtual DbSet<User> User { get; set; }

        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost;Database=inpatient_therapy;Trusted_Connection=True;");
            }
        }
        */

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasOne(d => d.AdlNavigation)
                    .WithMany(p => p.Appointment)
                    .HasForeignKey(d => d.Adl)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__appointment__adl__72910220");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Appointment)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__appointme__locat__719CDDE7");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Appointment)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__appointme__patie__70A8B9AE");

                entity.HasOne(d => d.Therapist)
                    .WithMany(p => p.Appointment)
                    .HasForeignKey(d => d.TherapistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__appointme__thera__6FB49575");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Appointment)
                    .HasForeignKey(d => new { d.RoomNumber, d.LocationId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__appointment__73852659");
            });

            modelBuilder.Entity<HoursWorked>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.HoursWorked)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__hours_wor__user___756D6ECB");
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Patient)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__patient__locatio__69FBBC1F");

                entity.HasOne(d => d.PmrPhysician)
                    .WithMany(p => p.Patient)
                    .HasForeignKey(d => d.PmrPhysicianId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__patient__pmr_phy__6BE40491");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Patient)
                    .HasForeignKey(d => new { d.RoomNumber, d.LocationId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__patient__6AEFE058");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.Role })
                    .HasName("PK__permissi__31DDE51B4CCCF9B9");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Permission)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__permissio__user___6CD828CA");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(e => new { e.Number, e.LocationId })
                    .HasName("PK__room__4A589D5E02FBB2E1");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Room)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__room__location_i__6EC0713C");
            });

            modelBuilder.Entity<TherapistEvent>(entity =>
            {
                entity.HasKey(e => e.EventId)
                    .HasName("PK__therapis__2370F727EA8F991A");

                entity.HasOne(d => d.Therapist)
                    .WithMany(p => p.TherapistEvent)
                    .HasForeignKey(d => d.TherapistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__therapist__thera__74794A92");
            });

            modelBuilder.Entity<Therapy>(entity =>
            {
                entity.HasKey(e => e.Adl)
                    .HasName("PK__therapy__DE50E69F96FEA1F1");

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.Therapy)
                    .HasForeignKey(d => d.Type)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__therapy__type__6DCC4D03");
            });

            modelBuilder.Entity<TherapyMain>(entity =>
            {
                entity.HasKey(e => e.Type)
                    .HasName("PK__therapy___E3F85249303EB6B3");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
