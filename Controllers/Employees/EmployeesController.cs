using AutoMapper;
using DataAcccessEFCore.Contracts;
using Microsoft.AspNetCore.Mvc;
using ModelsForAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmployeesAPI.Controllers.Employees
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeesController(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }
        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            try
            {
                return Ok(await _employeeRepository.GetAll());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data.");

            }

        }

        // GET api/Employees/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            try
            {
                var employee = await _employeeRepository.GetById(id);
                if (employee == null)
                {
                    return NotFound("Employee with Id ${id} not found.");
                }
                return Ok(employee);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data");
            }
        }

        // POST api/Employees
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            Console.WriteLine("FullName : " + employee.FullName);
            Console.WriteLine("Email : " + employee.Email);
            Console.WriteLine("Phone : " + employee.PhoneNumber);
            Console.WriteLine("DepartmentId : " + employee.DepartmentId);
            Console.WriteLine("GenderId : " + employee.GenderId);
            Console.WriteLine("PhotoPath : " + employee.PhotoPath);
            try
            {
               
                if (employee == null)
                {
                    return BadRequest("Invalid data");
                }

                var employeeToCreate = await _employeeRepository.Create(employee);                

                return CreatedAtAction(nameof(GetEmployee), new { id = employeeToCreate.EmployeeId }, employeeToCreate);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating data");
            }
        }

        // PUT api/Employees/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Employee>> PutEmployee(int id, Employee employee)
        {
            try
            {
                if (id != employee.EmployeeId)
                {
                    return BadRequest("Id mismatch");
                }

                var employeeToUpdate = await _employeeRepository.Find(e => e.EmployeeId == id);

                if (employeeToUpdate == null)
                {
                    return NotFound("Employee with Id ${id} not found.");
                }

                _mapper.Map(employee, employeeToUpdate);

                return Ok(await _employeeRepository.UpdateEntity(employeeToUpdate));
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating data");
            }
        }

        // DELETE api/Employees/5
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Employee>> Delete(int id)
        {
            try
            {
                var employeeToDelete = await _employeeRepository.Find(e => e.EmployeeId == id);
                if (employeeToDelete == null)
                {
                    return NotFound("Employee with Id ${id} not found.");
                }
                return Ok(await _employeeRepository.Delete(id));
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting data");
            }
        }

        // GET: api/Employees/searchKey
        [HttpGet("{searchKey}")]
        public async Task<ActionResult<IEnumerable<Employee>>> Search(string searchKey)
        {
            try
            {
                return Ok(await _employeeRepository.Search(e => e.Email.Contains(searchKey) ||
                                                           e.FullName.Contains(searchKey) ||
                                                           e.PhoneNumber.Contains(searchKey) ||
                                                           e.Department.Name.Contains(searchKey) ||
                                                           e.FullName.Contains(searchKey) ||
                                                           e.Gender.Name.Contains(searchKey)));
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data.");
            }

        }

    }
}
