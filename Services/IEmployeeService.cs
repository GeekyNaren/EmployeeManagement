using EmployeeManagement.Data.Dtos;
using EmployeeManagement.ExtensionService;

namespace EmployeeManagement.Services
{
    public interface IEmployeeService
    {
        Task<ServiceResponse<bool>> AddEmployee(AddEmployeeDto request);
        Task<ServiceResponse<List<EmployeeResponseDto>>> GetAllEmployees();
        Task<ServiceResponse<EmployeeResponseDto?>> GetEmployeeById(int employeeId);
        Task<ServiceResponse<bool>> UpdateEmployee(UpdateEmployeeDto request);
        Task<ServiceResponse<bool>> DeleteEmployee(int employeeId);
    }
}
