using Microsoft.EntityFrameworkCore;
using wbs_api.Models;

namespace wbs_api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Department> Departments { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Workstation> Workstations { get; set; } = null!;
    public DbSet<Booking> Bookings { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.ToTable("department");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.CreatedDate).HasColumnName("created_date");
            entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");
            entity.Property(e => e.IsActive).HasColumnName("isactive");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FullName).HasColumnName("full_name");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.Role).HasColumnName("role");
            entity.Property(e => e.EmployeeNumber).HasColumnName("employee_number");
            entity.Property(e => e.DepartmentName).HasColumnName("department_name");
            entity.Property(e => e.CreatedDate).HasColumnName("created_date");
            entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");
            entity.Property(e => e.IsActive).HasColumnName("isactive");
        });

        modelBuilder.Entity<Workstation>(entity =>
        {
            entity.ToTable("workstations");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasColumnName("code");

            entity.Property(e => e.XPosition)
                .HasColumnName("x_position");

            entity.Property(e => e.YPosition)
                .HasColumnName("y_position");

            entity.Property(e => e.Width)
                .HasColumnName("width");

            entity.Property(e => e.Height)
                .HasColumnName("height");

            entity.Property(e => e.IsActive)
                .HasColumnName("is_active");

            entity.Property(e => e.Wing)
                .HasColumnName("wing");

        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.ToTable("booking");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.WorkstationId)
                .HasColumnName("workstation_id");

            entity.Property(e => e.UserId)
            .HasColumnName("user_id");

            entity.Property(e => e.UserName)
                .HasColumnName("user_name");

            entity.Property(e => e.BookingDate)
                .HasColumnName("booking_date").HasColumnType("date"); 

            entity.Property(e => e.CreatedDate)
                .HasColumnName("created_date");

            entity.Property(e => e.UpdatedDate)
                .HasColumnName("updated_date");
            entity.Property(e => e.IsPermanent)
               .HasColumnName("is_permanent");
        });


        base.OnModelCreating(modelBuilder);
    }
}
