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
        public virtual DbSet<PatientActivity> PatientActivity { get; set; }
        public virtual DbSet<PatientEvent> PatientEvent { get; set; }
        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<Room> Room { get; set; }
        public virtual DbSet<TherapistActivity> TherapistActivity { get; set; }
        public virtual DbSet<TherapistEvent> TherapistEvent { get; set; }
        public virtual DbSet<Therapy> Therapy { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost;Database=inpatient_therapy;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.Property(e => e.AppointmentId).ValueGeneratedNever();

                entity.HasOne(d => d.AdlNavigation)
                    .WithMany(p => p.Appointment)
                    .HasForeignKey(d => d.Adl)
                    .HasConstraintName("FK__appointment__adl__3493CFA7");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Appointment)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK__appointme__locat__339FAB6E");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Appointment)
                    .HasForeignKey(d => d.PatientId)
                    .HasConstraintName("FK__appointme__patie__32AB8735");

                entity.HasOne(d => d.Therapist)
                    .WithMany(p => p.Appointment)
                    .HasForeignKey(d => d.TherapistId)
                    .HasConstraintName("FK__appointme__thera__31B762FC");
            });

            modelBuilder.Entity<HoursWorked>(entity =>
            {
                entity.Property(e => e.HoursWorkedId).ValueGeneratedNever();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HoursWorked)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__hours_wor__user___395884C4");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ__location__72E12F1BE9ADD21A")
                    .IsUnique();

                entity.Property(e => e.LocationId).ValueGeneratedNever();
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.Property(e => e.PatientId).ValueGeneratedNever();

                entity.HasOne(d => d.LocationNameNavigation)
                    .WithMany(p => p.Patient)
                    .HasPrincipalKey(p => p.Name)
                    .HasForeignKey(d => d.LocationName)
                    .HasConstraintName("FK__patient__locatio__2EDAF651");
            });

            modelBuilder.Entity<PatientActivity>(entity =>
            {
                entity.HasKey(e => e.ActivityName)
                    .HasName("PK__patient___656F4CCFA2CC132D");
            });

            modelBuilder.Entity<PatientEvent>(entity =>
            {
                entity.HasKey(e => e.EventId)
                    .HasName("PK__patient___2370F727C825D217");

                entity.Property(e => e.EventId).ValueGeneratedNever();

                entity.HasOne(d => d.ActivityNameNavigation)
                    .WithMany(p => p.PatientEvent)
                    .HasForeignKey(d => d.ActivityName)
                    .HasConstraintName("FK__patient_e__activ__3864608B");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.PatientEvent)
                    .HasForeignKey(d => d.PatientId)
                    .HasConstraintName("FK__patient_e__patie__37703C52");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.Role })
                    .HasName("PK__permissi__31DDE51B698429B6");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Permission)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__permissio__user___2FCF1A8A");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(e => new { e.Number, e.LocationId })
                    .HasName("PK__room__4A589D5E0AB79603");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Room)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__room__location_i__30C33EC3");
            });

            modelBuilder.Entity<TherapistActivity>(entity =>
            {
                entity.HasKey(e => e.ActivityName)
                    .HasName("PK__therapis__656F4CCF91479862");
            });

            modelBuilder.Entity<TherapistEvent>(entity =>
            {
                entity.HasKey(e => e.EventId)
                    .HasName("PK__therapis__2370F7273EA90970");

                entity.Property(e => e.EventId).ValueGeneratedNever();

                entity.HasOne(d => d.ActivityNameNavigation)
                    .WithMany(p => p.TherapistEvent)
                    .HasForeignKey(d => d.ActivityName)
                    .HasConstraintName("FK__therapist__activ__367C1819");

                entity.HasOne(d => d.Therapist)
                    .WithMany(p => p.TherapistEvent)
                    .HasForeignKey(d => d.TherapistId)
                    .HasConstraintName("FK__therapist__thera__3587F3E0");
            });

            modelBuilder.Entity<Therapy>(entity =>
            {
                entity.HasKey(e => e.Adl)
                    .HasName("PK__therapy__DE50E69F0C72921B");

                entity.HasIndex(e => e.Abbreviation)
                    .HasName("UQ__therapy__496A0484B424FAFD")
                    .IsUnique();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Username)
                    .HasName("UQ__user__F3DBC57250CD304B")
                    .IsUnique();

                entity.Property(e => e.UserId).ValueGeneratedNever();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
