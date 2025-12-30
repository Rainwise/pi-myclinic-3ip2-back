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

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<HealthRecord> HealthRecords { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Specialization> Specializations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=PI_Project;User=sa;Password=SQL;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK__Admin__B7C926381258014E");

            entity.ToTable("Admin");

            entity.HasIndex(e => e.Username, "UQ__Admin__536C85E4B4CA4821").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Admin__A9D105342E505914").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Username).HasMaxLength(100);
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.IdDoctor).HasName("PK__Doctors__F838DB3EE116E8E6");

            entity.HasIndex(e => e.Email, "UQ__Doctors__A9D105342BE350EB").IsUnique();

            entity.HasIndex(e => e.LicenseNumber, "UQ__Doctors__E8890166C28A5ABE").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.LicenseNumber).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);

            entity.HasOne(d => d.Specialization).WithMany(p => p.Doctors)
                .HasForeignKey(d => d.SpecializationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Doctors__Special__52593CB8");
        });

        modelBuilder.Entity<HealthRecord>(entity =>
        {
            entity.HasKey(e => e.IdHealthRecord).HasName("PK__HealthRe__14E9E5EF752BC6DD");

            entity.ToTable("HealthRecord");

            entity.HasOne(d => d.Patient).WithMany(p => p.HealthRecords)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("FK__HealthRec__Patie__6383C8BA");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.IdLog).HasName("PK__Log__0C54DBC6E26614DA");

            entity.ToTable("Log");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.IdPatient).HasName("PK__Patient__B7E7B5A4D99601C1");

            entity.ToTable("Patient");

            entity.HasIndex(e => e.Email, "UQ__Patient__A9D10534BDB790AB").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
        });

        modelBuilder.Entity<Specialization>(entity =>
        {
            entity.HasKey(e => e.IdSpecialization).HasName("PK__Speciali__78BB81BEF25D7611");

            entity.ToTable("Specialization");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
