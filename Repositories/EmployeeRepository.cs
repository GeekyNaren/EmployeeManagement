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
            _context.Employee.Update(employee);
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
