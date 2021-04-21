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
        public virtual DbSet<Authentication> Authentication { get; set; }
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
                    .HasConstraintName("FK__appointment__adl__2610A626");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Appointment)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__appointme__locat__251C81ED");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Appointment)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__appointme__patie__24285DB4");

                entity.HasOne(d => d.Therapist)
                    .WithMany(p => p.Appointment)
                    .HasForeignKey(d => d.TherapistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__appointme__thera__2334397B");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Appointment)
                    .HasForeignKey(d => new { d.RoomNumber, d.LocationId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__appointment__2704CA5F");
            });

            modelBuilder.Entity<Authentication>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Authentication)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__authentic__user___29E1370A");
            });

            modelBuilder.Entity<HoursWorked>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.HoursWorked)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__hours_wor__user___28ED12D1");
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Patient)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__patient__locatio__1C873BEC");

                entity.HasOne(d => d.PmrPhysician)
                    .WithMany(p => p.PatientPmrPhysician)
                    .HasForeignKey(d => d.PmrPhysicianId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__patient__pmr_phy__1E6F845E");

                entity.HasOne(d => d.Therapist)
                    .WithMany(p => p.PatientTherapist)
                    .HasForeignKey(d => d.TherapistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__patient__therapi__1F63A897");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Patient)
                    .HasForeignKey(d => new { d.RoomNumber, d.LocationId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__patient__1D7B6025");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.Role })
                    .HasName("PK__permissi__31DDE51B643F8246");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Permission)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__permissio__user___2057CCD0");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(e => new { e.Number, e.LocationId })
                    .HasName("PK__room__4A589D5EB62E4012");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Room)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__room__location_i__22401542");
            });

            modelBuilder.Entity<TherapistEvent>(entity =>
            {
                entity.HasKey(e => e.EventId)
                    .HasName("PK__therapis__2370F7279CB83471");

                entity.HasOne(d => d.Therapist)
                    .WithMany(p => p.TherapistEvent)
                    .HasForeignKey(d => d.TherapistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__therapist__thera__27F8EE98");
            });

            modelBuilder.Entity<Therapy>(entity =>
            {
                entity.HasKey(e => e.Adl)
                    .HasName("PK__therapy__DE50E69F439B277E");

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.Therapy)
                    .HasForeignKey(d => d.Type)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__therapy__type__214BF109");
            });

            modelBuilder.Entity<TherapyMain>(entity =>
            {
                entity.HasKey(e => e.Type)
                    .HasName("PK__therapy___E3F8524988AAB03D");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
