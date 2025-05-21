using EmployeeManagement.API.Data;
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

        // POST: api/employees
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] Employee employee)
        {
            // Encrypt DOB
            if (!DateTime.TryParse(employee.DateOfBirthEncrypted, out var dob))
                return BadRequest("Invalid date format for Date of Birth.");

            employee.DateOfBirthEncrypted = _encryptionService.Encrypt(dob.ToString("yyyy-MM-dd"));

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
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

            // Decrypt DOB before returning
            try
            {
                var decryptedDob = _encryptionService.Decrypt(employee.DateOfBirthEncrypted);
                employee.DateOfBirthEncrypted = decryptedDob;
            }
            catch
            {
                employee.DateOfBirthEncrypted = "Invalid Encrypted Data";
            }

            return Ok(employee);
        }

        // POST: api/employees/{employeeId}/jobpositions
        [HttpPost("{employeeId}/jobpositions")]
        public async Task<IActionResult> AddJobPosition(int employeeId, [FromBody] JobPosition jobPosition)
        {
            var employee = await _context.Employees
                .Include(e => e.JobPositions)
                .FirstOrDefaultAsync(e => e.Id == employeeId);

            if (employee == null)
                return NotFound("Employee not found.");

            if (jobPosition.Status.ToLower() == "active")
            {
                bool hasActive = employee.JobPositions.Any(j => j.Status.ToLower() == "active");
                if (hasActive)
                    return BadRequest("Employee already has an active job.");
            }

            jobPosition.EmployeeId = employeeId;
            _context.JobPositions.Add(jobPosition);
            await _context.SaveChangesAsync();

            return Ok(jobPosition);
        }
    }
}