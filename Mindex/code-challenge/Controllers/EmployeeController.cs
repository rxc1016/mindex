using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using challenge.Services;
using challenge.Models;

namespace challenge.Controllers
{
    [Route("api/employee")]
    public class EmployeeController : Controller
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;
        private readonly ICompensationService _compensationService;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService, ICompensationService compensationService)
        {
            _logger = logger;
            _employeeService = employeeService;
            _compensationService = compensationService;
        }

        [HttpPost]
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {
            _logger.LogDebug($"Received employee create request for '{employee.FirstName} {employee.LastName}'");

            _employeeService.Create(employee);

            return CreatedAtRoute("getEmployeeById", new { id = employee.EmployeeId }, employee);
        }

        [HttpGet("{id}", Name = "getEmployeeById")]
        public IActionResult GetEmployeeById(String id)
        {
            _logger.LogDebug($"Received employee get request for '{id}'");

            var employee = _employeeService.GetById(id);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPut("{id}")]
        public IActionResult ReplaceEmployee(String id, [FromBody]Employee newEmployee)
        {
            _logger.LogDebug($"Recieved employee update request for '{id}'");

            var existingEmployee = _employeeService.GetById(id);
            if (existingEmployee == null)
                return NotFound();

            _employeeService.Replace(existingEmployee, newEmployee);

            return Ok(newEmployee);
        }

        [HttpGet("{id}/reporting")]
        public IActionResult ReportingStructure(string id)
        {
            _logger.LogDebug($"Recieved reporting structure get request for '{id}'");

            var employee = _employeeService.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }

            var totalNumberOfReports = _employeeService.GetNumberOfEmployeeReports(employee);

            return Ok(new ReportingStructure
            {
                Employee = employee,
                NumberOfReports = totalNumberOfReports
            });
        }

        [HttpPost("{employeeId}/compensation")]
        public IActionResult CreateCompensation(string employeeId, [FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Recieved a Get request for compensation for employee '{employeeId}'");

            var existingEmployee = _employeeService.GetById(employeeId);
            if (existingEmployee == null)
                return NotFound();

            compensation.Employee = existingEmployee;
            _compensationService.Create(compensation);

            return Ok(compensation);
        }

        [HttpGet("{employeeId}/compensation")]
        public IActionResult GetCompensation(string employeeId)
        {
            _logger.LogDebug($"Recieved a Get request for compensation for employee '{employeeId}'");

            var existingEmployee = _employeeService.GetById(employeeId);
            if (existingEmployee == null)
                return NotFound();

            var compensation = _compensationService.GetByEmployeeId(employeeId);
            return Ok(compensation);
        }
    }
}