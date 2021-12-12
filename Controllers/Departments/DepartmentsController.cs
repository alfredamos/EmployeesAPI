using AutoMapper;
using DataAcccessEFCore.Contracts;
using Microsoft.AspNetCore.Mvc;
using ModelsForAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmployeesAPI.Controllers.Departments
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public DepartmentsController(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }
        // GET: api/departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            try
            {
                return Ok(await _departmentRepository.GetAll());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data.");
                
            }
            
        }

        // GET api/departments/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            try
            {
                var department = await _departmentRepository.GetById(id);
                if (department == null)
                {
                    return NotFound("Department with Id ${id} not found.");
                }
                return Ok(department);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data");
            }
        }

        // POST api/departments
        [HttpPost]
        public async Task<ActionResult<Department>> PostDepartment(Department department)
        {
            try
            {
                if (department == null)
                {
                    return BadRequest("Invalid data");
                }
                var departmentToCreate = await _departmentRepository.Create(department);

                return CreatedAtAction(nameof(GetDepartment), new { id = departmentToCreate.DepartmentId }, departmentToCreate);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating data");
            }
        }

        // PUT api/departments/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Department>> PutDepartment(int id, Department department)
        {            
            try
            {               
                if (id != department.DepartmentId)
                {
                    return BadRequest("Id mismatch");
                }                

                var departmentToUpdate = await _departmentRepository.GetById(id);
               
                if (departmentToUpdate == null)
                {
                    return NotFound("Department with Id ${id} not found.");
                }
                
                _mapper.Map(department, departmentToUpdate);
                
                return Ok(await _departmentRepository.UpdateEntity(departmentToUpdate));
            }
            catch (Exception)
            {                
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating data");
            }
        }

        // DELETE api/departments/5
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Department>> Delete(int id)
        {
            try
            {
                var departmentToDelete = await _departmentRepository.Find(d => d.DepartmentId == id);
                if (departmentToDelete == null)
                {
                    return NotFound("Department with Id ${id} not found.");
                }
                return Ok(await _departmentRepository.Delete(id));
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting data");
            }
        }

        // GET: api/departments/searchKey
        [HttpGet("{searchKey}")]
        public async Task<ActionResult<IEnumerable<Department>>> Search(string searchKey)
        {
            try
            {
                return Ok(await _departmentRepository.Search(d => d.Name.Contains(searchKey)));
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data.");
            }

        }
    }
}
