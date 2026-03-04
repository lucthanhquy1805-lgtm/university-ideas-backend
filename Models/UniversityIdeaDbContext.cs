using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace UniversityIdeas.API.Models;

public partial class UniversityIdeaDbContext : DbContext
{
    public UniversityIdeaDbContext()
    {
    }

    public UniversityIdeaDbContext(DbContextOptions<UniversityIdeaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AcademicYear> AcademicYears { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<Idea> Ideas { get; set; }

    public virtual DbSet<Reaction> Reactions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

   /* protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=UniversityIdeaDB;Trusted_Connection=True;TrustServerCertificate=True;");*/

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AcademicYear>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Academic__3214EC07E0C73EEE");

            entity.Property(e => e.ClosureDate).HasColumnType("datetime");
            entity.Property(e => e.FinalClosureDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC07F077C21F");

            entity.HasIndex(e => e.Name, "UQ__Categori__737584F614D10052").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comments__3214EC0700CE5A16");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsAnonymous).HasDefaultValue(false);

            entity.HasOne(d => d.Idea).WithMany(p => p.Comments)
                .HasForeignKey(d => d.IdeaId)
                .HasConstraintName("FK_Comments_Ideas");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comments_Users");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3214EC071E1D38F4");

            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Document__3214EC07F4A7112C");

            entity.Property(e => e.FileName).HasMaxLength(255);
            entity.Property(e => e.UploadedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Idea).WithMany(p => p.Documents)
                .HasForeignKey(d => d.IdeaId)
                .HasConstraintName("FK_Documents_Ideas");
        });

        modelBuilder.Entity<Idea>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ideas__3214EC0793B0EA4C");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsAnonymous).HasDefaultValue(false);
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.ViewCount).HasDefaultValue(0);

            entity.HasOne(d => d.AcademicYear).WithMany(p => p.Ideas)
                .HasForeignKey(d => d.AcademicYearId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ideas_AcademicYears");

            entity.HasOne(d => d.Category).WithMany(p => p.Ideas)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ideas_Categories");

            entity.HasOne(d => d.User).WithMany(p => p.Ideas)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ideas_Users");
        });

        modelBuilder.Entity<Reaction>(entity =>
        {
            entity.HasKey(e => new { e.IdeaId, e.UserId }).HasName("PK__Reaction__2F590EC7F6120A16");

            entity.HasOne(d => d.Idea).WithMany(p => p.Reactions)
                .HasForeignKey(d => d.IdeaId)
                .HasConstraintName("FK_Reactions_Ideas");

            entity.HasOne(d => d.User).WithMany(p => p.Reactions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reactions_Users");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC07CD8D48BF");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC072FC996EB");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D1053486E26AF0").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PasswordHash).IsUnicode(false);

            entity.HasOne(d => d.Department).WithMany(p => p.Users)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Departments");

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK_UserRoles_Roles"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_UserRoles_Users"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId").HasName("PK__UserRole__AF2760AD46C74D6D");
                        j.ToTable("UserRoles");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
