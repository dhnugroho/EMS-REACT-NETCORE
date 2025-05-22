namespace EmployeeManagement.API.Dtos
{
    public class CreateJobPositionDto
    {
        public string JobName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal Salary { get; set; }
        public string Status { get; set; } = "active";
    }

    public class JobPositionDto
    {
        public int Id { get; set; }
        public string JobName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal Salary { get; set; }
        public string Status { get; set; } = "active";
    }
}
