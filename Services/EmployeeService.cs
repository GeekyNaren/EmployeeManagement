using EmployeeManagement.Data.Dtos;
using EmployeeManagement.Data.Entities;
using EmployeeManagement.ExtensionService;
using EmployeeManagement.Repositories;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace EmployeeManagement.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;

        #region public methods
        public EmployeeService(IEmployeeRepository employeeRepository, ILogger<EmployeeService> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }
        public async Task<ServiceResponse<bool>> AddEmployee(AddEmployeeDto request)
        {
            var errors = ValidateAddRequest(request);
            if (errors.Any())
            {
                return ServiceResponse<bool>.Fail(errors, "Validation failed");
            }

            var employeeList = await _employeeRepository.GetAllEmployees();
            // Check duplicate mobile
            if (employeeList.Any(e => e.MobileNo == request.MobileNo))
            {
                return ServiceResponse<bool>.Fail("Employee with same mobile number already exists");
            }
            // check duplicate email
            if (employeeList.Any(e => e.EmailId.Equals(request.EmailId, StringComparison.OrdinalIgnoreCase)))
            {
                return ServiceResponse<bool>.Fail("Employee with same email already exists");
            }
            // check duplicate PAN
            if (!string.IsNullOrWhiteSpace(request.PANCardNo) && employeeList.Any(e => e.PANCardNo != null && e.PANCardNo.Equals(request.PANCardNo, StringComparison.OrdinalIgnoreCase)))
            {
                return ServiceResponse<bool>.Fail("Employee with same PAN card number already exists");
            }
            var employee = new Employee
            {
                EmployeeName = request.EmployeeName,
                MobileNo = request.MobileNo,
                EmailId = request.EmailId,
                PANCardNo = request.PANCardNo?.ToUpper(),
                JoiningDate = request.JoiningDate,
                PreviousCompanyLastWorkingDate = request.PreviousCompanyLastWorkingDate,
                Education = request.Education
            };

            await _employeeRepository.AddEmployee(employee);
            return ServiceResponse<bool>.Ok(true);
        }
        public async Task<ServiceResponse<PagedResponse<EmployeeResponseDto>>> GetAllEmployees(int pageNumber = 1, int pageSize = 5)
        {
            var employees = (await _employeeRepository.GetAllEmployees()).ToList().OrderByDescending(e => e.EmployeeId);
            var employeeDtos = employees.Select(e => new EmployeeResponseDto
            {
                EmployeeId = e.EmployeeId,
                EmployeeName = e.EmployeeName,
                MobileNo = e.MobileNo,
                EmailId = e.EmailId,
                PANCardNo = e.PANCardNo.ToUpper(),
                JoiningDate = e.JoiningDate,
                PreviousCompanyLastWorkingDate = e.PreviousCompanyLastWorkingDate,
                Education = e.Education
            }).ToList();

            var totalRecords = employeeDtos.Count;
            if (pageSize < 1) pageSize = 5;
            if (pageNumber < 1) pageNumber = 1;
            var totalPages = pageSize > 0 ? (int)Math.Ceiling(totalRecords / (double)pageSize) : 1;

            var pagedData = employeeDtos.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            var pagedResponse = new PagedResponse<EmployeeResponseDto>
            {
                Data = pagedData,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages
            };

            return ServiceResponse<PagedResponse<EmployeeResponseDto>>.Ok(pagedResponse);
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
                PANCardNo = employee.PANCardNo.ToUpper(),
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
            var errors = ValidateUpdateRequest(request, employee);
            if (errors.Any())
            {
                return ServiceResponse<bool>.Fail(errors, "Validation failed");
            }

            var employeeList = await _employeeRepository.GetAllEmployees();
            // If mobile changed, ensure uniqueness
            if (!string.IsNullOrWhiteSpace(request.MobileNo) && request.MobileNo != employee.MobileNo)
            {
                if (employeeList.Any(e => e.MobileNo == request.MobileNo && e.EmployeeId != employee.EmployeeId))
                {
                    return ServiceResponse<bool>.Fail("Employee with same mobile number already exists");
                }
                employee.MobileNo = request.MobileNo;
            }

            // If email changed, ensure uniqueness
            if (!string.IsNullOrWhiteSpace(request.EmailId) && request.EmailId != employee.EmailId)
            {
                if (employeeList.Any(e => e.EmailId.Equals(request.EmailId, StringComparison.OrdinalIgnoreCase) && e.EmployeeId != employee.EmployeeId))
                {
                    return ServiceResponse<bool>.Fail("Employee with same email already exists");
                }
                employee.EmailId = request.EmailId;
            }

            // If PAN changed, ensure uniqueness
            //if (!string.IsNullOrWhiteSpace(request.PANCardNo) && request.PANCardNo != employee.PANCardNo)
            if (!string.IsNullOrWhiteSpace(request.PANCardNo) && !string.Equals(request.PANCardNo, employee.PANCardNo, StringComparison.OrdinalIgnoreCase))
            {
                if (employeeList.Any(e => e.PANCardNo != null && e.PANCardNo.Equals(request.PANCardNo, StringComparison.OrdinalIgnoreCase) && e.EmployeeId != employee.EmployeeId))
                {
                    return ServiceResponse<bool>.Fail("Employee with same PAN card number already exists");
                }
                employee.PANCardNo = request.PANCardNo.ToUpper();
            }

            if (!string.IsNullOrWhiteSpace(request.EmployeeName)) employee.EmployeeName = request.EmployeeName;
            if (!string.IsNullOrWhiteSpace(request.EmailId)) employee.EmailId = request.EmailId;
            if (request.PANCardNo != null) employee.PANCardNo = request.PANCardNo.ToUpper();
            employee.JoiningDate = request.JoiningDate ?? employee.JoiningDate;
            employee.PreviousCompanyLastWorkingDate = request.PreviousCompanyLastWorkingDate;
            if (!string.IsNullOrWhiteSpace(request.Education)) employee.Education = request.Education;

            await _employeeRepository.UpdateEmployee(employee);
            return ServiceResponse<bool>.Ok(true);
        }
        #endregion

        #region private methods
        private List<string> ValidateAddRequest(AddEmployeeDto request)
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(request.EmployeeName))
                errors.Add("EmployeeName is mandatory");

            if (string.IsNullOrWhiteSpace(request.MobileNo))
                errors.Add("MobileNo is mandatory");
            else
            {
                if (!Regex.IsMatch(request.MobileNo, "^[0-9]{10}$"))
                    errors.Add("MobileNo must be 10 digits");
                else if (!Regex.IsMatch(request.MobileNo, "^[789][0-9]{9}$"))
                    errors.Add("MobileNo must start with 7, 8 or 9");
            }

            if (string.IsNullOrWhiteSpace(request.EmailId))
                errors.Add("EmailId is mandatory");
            else
            {
                try
                {
                    var addr = new MailAddress(request.EmailId);
                }
                catch
                {
                    errors.Add("EmailId is not valid");
                }
            }

            if (!string.IsNullOrWhiteSpace(request.PANCardNo))
            {
                // PAN format: 5 letters, 4 digits, 1 letter
                if (!Regex.IsMatch(request.PANCardNo, "^[A-Za-z]{5}[0-9]{4}[A-Za-z]{1}$"))
                    errors.Add("PANCardNo is not valid");
            }

            if (request.JoiningDate == default)
                errors.Add("JoiningDate is mandatory");

            if (request.PreviousCompanyLastWorkingDate.HasValue && request.PreviousCompanyLastWorkingDate > request.JoiningDate)
                errors.Add("PreviousCompanyLastWorkingDate cannot be greater than JoiningDate");

            //var allowed = new[] { "BCA", "BCS", "BSc", "MCA", "MBA", "Phd", "Other" };
            //if (string.IsNullOrWhiteSpace(request.Education) || !allowed.Contains(request.Education))
            //    errors.Add("Education is mandatory and must be one of the allowed values");

            return errors;
        }
        private List<string> ValidateUpdateRequest(UpdateEmployeeDto request, Employee existing)
        {
            var errors = new List<string>();
            if (request.EmployeeName != null && string.IsNullOrWhiteSpace(request.EmployeeName))
                errors.Add("EmployeeName is mandatory");

            if (request.MobileNo != null)
            {
                if (!Regex.IsMatch(request.MobileNo, "^[0-9]{10}$"))
                    errors.Add("MobileNo must be 10 digits");
                else if (!Regex.IsMatch(request.MobileNo, "^[789][0-9]{9}$"))
                    errors.Add("MobileNo must start with 7, 8 or 9");
            }

            if (request.EmailId != null)
            {
                try
                {
                    var addr = new MailAddress(request.EmailId);
                }
                catch
                {
                    errors.Add("EmailId is not valid");
                }
            }

            if (request.PANCardNo != null && !string.IsNullOrWhiteSpace(request.PANCardNo))
            {
                if (!Regex.IsMatch(request.PANCardNo, "^[A-Za-z]{5}[0-9]{4}[A-Za-z]{1}$"))
                    errors.Add("PANCardNo is not valid");
            }

            var joiningDate = request.JoiningDate ?? existing.JoiningDate;
            if (request.PreviousCompanyLastWorkingDate.HasValue && request.PreviousCompanyLastWorkingDate > joiningDate)
                errors.Add("PreviousCompanyLastWorkingDate cannot be greater than JoiningDate");

            //if (request.Education != null)
            //{
            //    var allowed = new[] { "BCA", "BCS", "BSc", "MCA", "MBA", "Phd", "Other" };
            //    if (!allowed.Contains(request.Education))
            //        errors.Add("Education must be one of the allowed values");
            //}

            return errors;
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
        #endregion
    }
}
