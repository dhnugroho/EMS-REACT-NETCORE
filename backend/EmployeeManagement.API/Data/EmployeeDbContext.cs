using Microsoft.EntityFrameworkCore;
using EmployeeManagement.API.Models;
using EmployeeManagement.API.Dtos;

namespace EmployeeManagement.API.Data
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<JobPosition> JobPositions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JobPosition>()
                .Property(j => j.Salary)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<JobPosition>()
                .HasOne(j => j.Employee)
                .WithMany(e => e.JobPositions)
                .HasForeignKey(j => j.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);  // Optional: Cascade delete if employee is deleted

            // Ensure only one active job per employee
            // modelBuilder.Entity<JobPosition>()
            //     .HasIndex(j => new { j.EmployeeId, j.Status })
            //     .IsUnique()
            //     .HasFilter("[status] = active");  // Only enforce uniqueness for active jobs

            // Ensure only one active job per employee (optional - could enforce via service layer)
        }

        public async Task<List<EmployeeDto>> GetAllEmployeesAsync()
        {
            return await Employees
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    FirstName = e.FirstName,
                    MiddleName = e.MiddleName,
                    LastName = e.LastName,
                    DateOfBirth = e.DateOfBirthEncrypted,
                    Gender = e.Gender,
                    Address = e.Address
                })
                .ToListAsync();
        }
    }
}
