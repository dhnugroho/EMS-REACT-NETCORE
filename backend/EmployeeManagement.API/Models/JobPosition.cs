using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.API.Models
{
    public class JobPosition
    {
        public int Id { get; set; }

        [Required]
        public string JobName { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        public decimal Salary { get; set; }

        [Required]
        public string Status { get; set; } = "active"; // active or inactive

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;
    }
}
