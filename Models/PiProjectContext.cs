using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace myclinic_back.Models;

public partial class PiProjectContext : DbContext
{
    public PiProjectContext()
    {
    }

    public PiProjectContext(DbContextOptions<PiProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<HealthRecord> HealthRecords { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Specialization> Specializations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=PI_Project;User Id=sa;Password=SQL;TrustServerCertificate=True;Trusted_Connection=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.IdAccount).HasName("PK__Account__B7B00CE3A674AA28");

            entity.ToTable("Account");

            entity.Property(e => e.EmailAddress).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(30);
            entity.Property(e => e.LastName).HasMaxLength(30);

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Account__RoleId__08B54D69");
        });

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Admin__349DA5A6BBA247BA");

            entity.ToTable("Admin");

            entity.Property(e => e.AccountId).ValueGeneratedNever();

            entity.HasOne(d => d.Account).WithOne(p => p.Admin)
                .HasForeignKey<Admin>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Admin__AccountId__123EB7A3");
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.IdAppointment).HasName("PK__Appointm__ECE24AABFCB899DA");

            entity.ToTable("Appointment");

            entity.Property(e => e.CreatedAt).HasColumnName("Created_at");
            entity.Property(e => e.StartsAt).HasColumnName("Starts_at");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.DoctorId)
                .HasConstraintName("FK__Appointme__Docto__17F790F9");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Doctor__349DA5A65B8692F6");

            entity.ToTable("Doctor");

            entity.Property(e => e.AccountId).ValueGeneratedNever();

            entity.HasOne(d => d.Account).WithOne(p => p.Doctor)
                .HasForeignKey<Doctor>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Doctor__AccountI__0B91BA14");

            entity.HasOne(d => d.Specialization).WithMany(p => p.Doctors)
                .HasForeignKey(d => d.SpecializationId)
                .HasConstraintName("FK__Doctor__Speciali__0C85DE4D");
        });

        modelBuilder.Entity<HealthRecord>(entity =>
        {
            entity.HasKey(e => e.IdHealthRecord).HasName("PK__HealthRe__14E9E5EF7D161587");

            entity.ToTable("HealthRecord");

            entity.HasOne(d => d.Patient).WithMany(p => p.HealthRecords)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("FK__HealthRec__Patie__151B244E");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.IdNotification).HasName("PK__Notifica__950094B17CFE0E10");

            entity.ToTable("Notification");

            entity.HasOne(d => d.Reservation).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.ReservationId)
                .HasConstraintName("FK__Notificat__Reser__1EA48E88");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Patient__349DA5A6D3386D33");

            entity.ToTable("Patient");

            entity.Property(e => e.AccountId).ValueGeneratedNever();

            entity.HasOne(d => d.Account).WithOne(p => p.Patient)
                .HasForeignKey<Patient>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Patient__Account__0F624AF8");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.IdReservation).HasName("PK__Reservat__7E69A57BC277C5AD");

            entity.ToTable("Reservation");

            entity.Property(e => e.CreatedAt).HasColumnName("created_at");

            entity.HasOne(d => d.Appointment).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.AppointmentId)
                .HasConstraintName("FK__Reservati__Appoi__1AD3FDA4");

            entity.HasOne(d => d.Patient).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("FK__Reservati__Patie__1BC821DD");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("PK__Role__B4369054607FE74D");

            entity.ToTable("Role");

            entity.HasIndex(e => e.Name, "UQ__Role__737584F6836D34C6").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(30);
        });

        modelBuilder.Entity<Specialization>(entity =>
        {
            entity.HasKey(e => e.IdSpecialization).HasName("PK__Speciali__78BB81BE5427853C");

            entity.ToTable("Specialization");

            entity.HasIndex(e => e.Name, "UQ__Speciali__737584F6B0EF3A7F").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
