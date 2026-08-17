using wbs_department_api.Models;
using Microsoft.EntityFrameworkCore;

namespace wbs_department_api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Department> Departments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("department");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasColumnName("name");

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
