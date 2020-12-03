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
        public virtual DbSet<RoomNumber> RoomNumber { get; set; }
        public virtual DbSet<TherapistActivity> TherapistActivity { get; set; }
        public virtual DbSet<TherapistEvent> TherapistEvent { get; set; }
        public virtual DbSet<Therapy> Therapy { get; set; }
        public virtual DbSet<User> User { get; set; }

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost;Database=inpatient_therapy;Trusted_Connection=True;");
            }
        }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(e => new { e.StartDatetime, e.EndDatetime, e.TherapistId, e.PatientId })
                    .HasName("PK__appointm__EA549AA12F3B707F");

                entity.HasOne(d => d.AdlNavigation)
                    .WithMany(p => p.Appointment)
                    .HasForeignKey(d => d.Adl)
                    .HasConstraintName("FK__appointment__adl__3F466844");

                entity.HasOne(d => d.L)
                    .WithMany(p => p.Appointment)
                    .HasForeignKey(d => d.Lid)
                    .HasConstraintName("FK__appointment__lid__3E52440B");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Appointment)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__appointme__patie__3D5E1FD2");

                entity.HasOne(d => d.Therapist)
                    .WithMany(p => p.Appointment)
                    .HasForeignKey(d => d.TherapistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__appointme__thera__3C69FB99");
            });

            modelBuilder.Entity<HoursWorked>(entity =>
            {
                entity.HasKey(e => new { e.StartDatetime, e.EndDatetime, e.Uid })
                    .HasName("PK__hours_wo__18EBE0A61C065113");

                entity.HasOne(d => d.U)
                    .WithMany(p => p.HoursWorked)
                    .HasForeignKey(d => d.Uid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__hours_worke__uid__440B1D61");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(e => e.Lid)
                    .HasName("PK__location__DE105D07A5ED91B7");

                entity.Property(e => e.Lid).ValueGeneratedNever();
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.Pid)
                    .HasName("PK__patient__DD37D91AC86EBF63");

                entity.Property(e => e.Pid).ValueGeneratedNever();
            });

            modelBuilder.Entity<PatientActivity>(entity =>
            {
                entity.HasKey(e => e.Name)
                    .HasName("PK__patient___72E12F1A1C543A69");
            });

            modelBuilder.Entity<PatientEvent>(entity =>
            {
                entity.HasKey(e => new { e.StartDatetime, e.EndDatetime, e.PatientId })
                    .HasName("PK__patient___0A7BCC5031538EC3");

                entity.HasOne(d => d.ActivityNavigation)
                    .WithMany(p => p.PatientEvent)
                    .HasForeignKey(d => d.Activity)
                    .HasConstraintName("FK__patient_e__activ__4316F928");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.PatientEvent)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__patient_e__patie__4222D4EF");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Role })
                    .HasName("PK__permissi__BA703A2B1EEFED11");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.Permission)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__permission__id__3A81B327");
            });

            modelBuilder.Entity<RoomNumber>(entity =>
            {
                entity.HasKey(e => new { e.Number, e.Lid })
                    .HasName("PK__room_num__90C81B90323A19D5");

                entity.HasOne(d => d.L)
                    .WithMany(p => p.RoomNumber)
                    .HasForeignKey(d => d.Lid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__room_number__lid__3B75D760");
            });

            modelBuilder.Entity<TherapistActivity>(entity =>
            {
                entity.HasKey(e => e.Name)
                    .HasName("PK__therapis__72E12F1A2A663DCA");
            });

            modelBuilder.Entity<TherapistEvent>(entity =>
            {
                entity.HasKey(e => new { e.StartDatetime, e.EndDatetime, e.TherapistId })
                    .HasName("PK__therapis__AD204F6FA31E1C20");

                entity.HasOne(d => d.ActivityNavigation)
                    .WithMany(p => p.TherapistEvent)
                    .HasForeignKey(d => d.Activity)
                    .HasConstraintName("FK__therapist__activ__412EB0B6");

                entity.HasOne(d => d.Therapist)
                    .WithMany(p => p.TherapistEvent)
                    .HasForeignKey(d => d.TherapistId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__therapist__thera__403A8C7D");
            });

            modelBuilder.Entity<Therapy>(entity =>
            {
                entity.HasKey(e => e.Adl)
                    .HasName("PK__therapy__DE50E69F39E7D9B6");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Uid)
                    .HasName("PK__user__DD7012640DA3938A");

                entity.Property(e => e.Uid).ValueGeneratedNever();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
