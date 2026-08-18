using EmployeeManagement.Data.Dtos;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost("AddEmployee")]
        public async Task<ActionResult> AddEmployee([FromBody] AddEmployeeDto request)
        {
            var response = await _employeeService.AddEmployee(request);
            if (response == null || !response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetAllEmployees")]
        public async Task<ActionResult> GetAllEmployees()
        {
            var response = await _employeeService.GetAllEmployees();
            return Ok(response);
        }

        [HttpGet("GetEmployeeById")]
        public async Task<ActionResult> GetEmployeeById(int employeeId)
        {
            var response = await _employeeService.GetEmployeeById(employeeId);
            if (response == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPut("UpdateEmployee")]
        public async Task<ActionResult> UpdateEmployee([FromBody] UpdateEmployeeDto request)
        {
            var response = await _employeeService.UpdateEmployee(request);
            if (response == null || !response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("DeleteEmployee")]
        public async Task<ActionResult> DeleteEmployee(int employeeId)
        {
            var response = await _employeeService.DeleteEmployee(employeeId);
            if (response == null || !response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
