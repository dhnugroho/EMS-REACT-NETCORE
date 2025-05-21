using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.API.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        public string? MiddleName { get; set; }

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string DateOfBirthEncrypted { get; set; } = string.Empty;

        [Required]
        public string Gender { get; set; } = string.Empty;

        public string? Address { get; set; }

        public ICollection<JobPosition> JobPositions { get; set; } = new List<JobPosition>();
    }
}
