namespace EmployeeManagement.API.Dtos
{
    public class CreateEmployeeDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string DateOfBirth { get; set; } = string.Empty; // Plaintext ISO date
        public string Gender { get; set; } = string.Empty;
        public string? Address { get; set; }
    }

    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string DateOfBirth { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string? Address { get; set; }

        public List<JobPositionDto> JobPositions { get; set; } = new();
    }
}
