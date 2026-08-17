using Microsoft.EntityFrameworkCore;
using wbs_user_api.Models;

namespace wbs_user_api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.FullName)
                    .HasColumnName("full_name");

                entity.Property(e => e.Email)
                    .HasColumnName("email");

                entity.Property(e => e.PasswordHash)
                    .HasColumnName("password_hash");

                entity.Property(e => e.EmployeeNumber)
                    .HasColumnName("employee_number");

                entity.Property(e => e.DepartmentName)
                    .HasColumnName("department_name");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnName("updated_date");

                entity.Property(e => e.IsActive)
                    .HasColumnName("isactive");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
