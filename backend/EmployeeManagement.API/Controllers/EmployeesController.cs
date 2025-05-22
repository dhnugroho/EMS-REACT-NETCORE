using EmployeeManagement.API.Data;
using EmployeeManagement.API.Dtos;
using EmployeeManagement.API.Models;
using EmployeeManagement.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeDbContext _context;
        private readonly EncryptionService _encryptionService;

        public EmployeesController(EmployeeDbContext context, EncryptionService encryptionService)
        {
            _context = context;
            _encryptionService = encryptionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
        {
            // Assuming you have a method in your DbContext or repository to get all employees
            var employees = await _context.GetAllEmployeesAsync();

            return Ok(employees);
        }

        // POST: api/employees
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeDto dto)
        {
            if (!DateTime.TryParse(dto.DateOfBirth, out var dob))
                return BadRequest("Invalid date format for Date of Birth.");

            var employee = new Employee
            {
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                LastName = dto.LastName,
                Gender = dto.Gender,
                Address = dto.Address,
                DateOfBirthEncrypted = _encryptionService.Encrypt(dob.ToString("yyyy-MM-dd"))
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, new { employee.Id });
        }

        // GET: api/employees/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.JobPositions)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
                return NotFound();

            string decryptedDob;
            try
            {
                decryptedDob = _encryptionService.Decrypt(employee.DateOfBirthEncrypted);
            }
            catch
            {
                decryptedDob = "Invalid Encrypted Data";
            }

            var dto = new EmployeeDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
                LastName = employee.LastName,
                Gender = employee.Gender,
                Address = employee.Address,
                DateOfBirth = decryptedDob,
                JobPositions = employee.JobPositions.Select(j => new JobPositionDto
                {
                    Id = j.Id,
                    JobName = j.JobName,
                    StartDate = j.StartDate,
                    EndDate = j.EndDate,
                    Salary = j.Salary,
                    Status = j.Status
                }).ToList()
            };

            return Ok(dto);
        }

        // POST: api/employees/{employeeId}/jobpositions
        [HttpPost("{employeeId}/jobpositions")]
        public async Task<IActionResult> AddJobPosition(int employeeId, [FromBody] CreateJobPositionDto dto)
        {
            var employee = await _context.Employees
                .Include(e => e.JobPositions)
                .FirstOrDefaultAsync(e => e.Id == employeeId);

            if (employee == null)
                return NotFound("Employee not found.");

            if (dto.Status.ToLower() == "active")
            {
                bool hasActive = employee.JobPositions.Any(j => j.Status.ToLower() == "active");
                if (hasActive)
                    return BadRequest("Employee already has an active job.");
            }

            var job = new JobPosition
            {
                JobName = dto.JobName,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Salary = dto.Salary,
                Status = dto.Status,
                EmployeeId = employeeId
            };

            _context.JobPositions.Add(job);
            await _context.SaveChangesAsync();

            return Ok(new { job.Id });
        }
    }
}
