using EmployeeManagement.Data;
using EmployeeManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;
        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployees()
        {
            return await _context.Employee.ToListAsync();
        }
        public async Task<Employee?> GetEmployeeById(int employeeId)
        {
            return await _context.Employee.FindAsync(employeeId);
        }
        public async Task AddEmployee(Employee employee)
        {
            _context.Employee.Add(employee);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateEmployee(Employee employee)
        {
            // Retrieve the existing entity so we only modify the fields provided by the caller.
            var existing = await _context.Employee.FindAsync(employee.EmployeeId);
            if (existing == null)
            {
                return;
            }

            if (!string.IsNullOrEmpty(employee.EmployeeName))
                existing.EmployeeName = employee.EmployeeName;

            if (!string.IsNullOrEmpty(employee.MobileNo))
                existing.MobileNo = employee.MobileNo;

            if (!string.IsNullOrEmpty(employee.EmailId))
                existing.EmailId = employee.EmailId;

            if (employee.PANCardNo != null)
                existing.PANCardNo = employee.PANCardNo;

            if (employee.JoiningDate != default(DateOnly))
                existing.JoiningDate = employee.JoiningDate;

            if (employee.PreviousCompanyLastWorkingDate.HasValue)
                existing.PreviousCompanyLastWorkingDate = employee.PreviousCompanyLastWorkingDate;

            if (!string.IsNullOrEmpty(employee.Education))
                existing.Education = employee.Education;

            await _context.SaveChangesAsync();
        }
        public async Task DeleteEmployee(int employeeId)
        {
            var employee = await _context.Employee.FindAsync(employeeId);
            if (employee != null)
            {
                _context.Employee.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }
    }
}
