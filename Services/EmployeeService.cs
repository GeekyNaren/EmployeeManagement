using EmployeeManagement.Data.Dtos;
using EmployeeManagement.Data.Entities;
using EmployeeManagement.ExtensionService;
using EmployeeManagement.Repositories;

namespace EmployeeManagement.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;
        public EmployeeService(IEmployeeRepository employeeRepository, ILogger<EmployeeService> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }
        public async Task<ServiceResponse<bool>> AddEmployee(AddEmployeeDto request)
        {
            var employeeList = await _employeeRepository.GetAllEmployees();

            var employee = new Employee
            {
                EmployeeName = request.EmployeeName,
                MobileNo = request.MobileNo,
                EmailId = request.EmailId,
                PANCardNo = request.PANCardNo,
                JoiningDate = request.JoiningDate,
                PreviousCompanyLastWorkingDate = request.PreviousCompanyLastWorkingDate,
                Education = request.Education
            };

            await _employeeRepository.AddEmployee(employee);
            return ServiceResponse<bool>.Ok(true);
        }
        public async Task<ServiceResponse<List<EmployeeResponseDto>>> GetAllEmployees()
        {
            var employees = await _employeeRepository.GetAllEmployees();
            var employeeDtos = employees.Select(e => new EmployeeResponseDto
            {
                EmployeeId = e.EmployeeId,
                EmployeeName = e.EmployeeName,
                MobileNo = e.MobileNo,
                EmailId = e.EmailId,
                PANCardNo = e.PANCardNo,
                JoiningDate = e.JoiningDate,
                PreviousCompanyLastWorkingDate = e.PreviousCompanyLastWorkingDate,
                Education = e.Education
            }).ToList();
            return ServiceResponse<List<EmployeeResponseDto>>.Ok(employeeDtos);
        }
        public async Task<ServiceResponse<EmployeeResponseDto?>> GetEmployeeById(int employeeId)
        {
            var employee = await _employeeRepository.GetEmployeeById(employeeId);
            if (employee == null)
            {
                return ServiceResponse<EmployeeResponseDto?>.Fail("Employee not found");
            }
            var employeeDto = new EmployeeResponseDto
            {
                EmployeeId = employee.EmployeeId,
                EmployeeName = employee.EmployeeName,
                MobileNo = employee.MobileNo,
                EmailId = employee.EmailId,
                PANCardNo = employee.PANCardNo,
                JoiningDate = employee.JoiningDate,
                PreviousCompanyLastWorkingDate = employee.PreviousCompanyLastWorkingDate,
                Education = employee.Education
            };
            return ServiceResponse<EmployeeResponseDto?>.Ok(employeeDto);
        }
        public async Task<ServiceResponse<bool>> UpdateEmployee(UpdateEmployeeDto request)
        {
            var employee = await _employeeRepository.GetEmployeeById(request.EmployeeId);
            if (employee == null)
            {
                return ServiceResponse<bool>.Fail("Employee not found");
            }
            employee.EmployeeName = request.EmployeeName;
            employee.MobileNo = request.MobileNo;
            employee.EmailId = request.EmailId;
            employee.PANCardNo = request.PANCardNo;
            employee.JoiningDate = request.JoiningDate ?? employee.JoiningDate;
            employee.PreviousCompanyLastWorkingDate = request.PreviousCompanyLastWorkingDate;
            employee.Education = request.Education;
            await _employeeRepository.UpdateEmployee(employee);
            return ServiceResponse<bool>.Ok(true);
        }
        public async Task<ServiceResponse<bool>> DeleteEmployee(int employeeId)
        {
            var employee = await _employeeRepository.GetEmployeeById(employeeId);
            if (employee == null)
            {
                return ServiceResponse<bool>.Fail("Employee not found");
            }
            await _employeeRepository.DeleteEmployee(employeeId);
            return ServiceResponse<bool>.Ok(true);
        }
    }
}
