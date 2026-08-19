using EmployeeManagement.Data.Dtos;
using EmployeeManagement.ExtensionService;

namespace EmployeeManagement.Services
{
    public interface IEmployeeService
    {
        Task<ServiceResponse<bool>> AddEmployee(AddEmployeeDto request);
        Task<ServiceResponse<PagedResponse<EmployeeResponseDto>>> GetAllEmployees(int pageNumber = 1, int pageSize = 5);
        Task<ServiceResponse<EmployeeResponseDto?>> GetEmployeeById(int employeeId);
        Task<ServiceResponse<bool>> UpdateEmployee(UpdateEmployeeDto request);
        Task<ServiceResponse<bool>> DeleteEmployee(int employeeId);
    }
}
