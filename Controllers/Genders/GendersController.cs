using AutoMapper;
using DataAcccessEFCore.Contracts;
using Microsoft.AspNetCore.Mvc;
using ModelsForAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EmployeesAPI.Controllers.Genders
{
    [Route("api/[controller]")]
    [ApiController]
    public class GendersController : ControllerBase
    {
        private readonly IGenderRepository _genderRepository;
        private readonly IMapper _mapper;

        public GendersController(IGenderRepository genderRepository, IMapper mapper)
        {
            _genderRepository = genderRepository;
            _mapper = mapper;
        }
        // GET: api/Genders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gender>>> GetGenders()
        {
            try
            {
                return Ok(await _genderRepository.GetAll());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data.");

            }

        }

        // GET api/Genders/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Gender>> GetGender(int id)
        {
            try
            {
                var gender = await _genderRepository.GetById(id);
                if (gender == null)
                {
                    return NotFound("Gender with Id ${id} not found.");
                }
                return Ok(gender);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data");
            }
        }

        // POST api/Genders
        [HttpPost]
        public async Task<ActionResult<Gender>> PostGender(Gender gender)
        {
            try
            {
                if (gender == null)
                {
                    return BadRequest("Invalid data");
                }
                var genderToCreate = await _genderRepository.Create(gender);

                return CreatedAtAction(nameof(GetGender), new { id = genderToCreate.GenderId }, genderToCreate);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating data");
            }
        }

        // PUT api/Genders/5
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Gender>> PutGender(int id, Gender gender)
        {
            try
            {
                if (id != gender.GenderId)
                {
                    return BadRequest("Id mismatch");
                }

                var genderToUpdate = await _genderRepository.GetById(id);

                if (genderToUpdate == null)
                {
                    return NotFound("Gender with Id ${id} not found.");
                }

                _mapper.Map(gender, genderToUpdate);

                return Ok(await _genderRepository.UpdateEntity(genderToUpdate));
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating data");
            }
        }

        // DELETE api/Genders/5
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Gender>> Delete(int id)
        {
            try
            {
                var genderToDelete = await _genderRepository.Find(d => d.GenderId == id);
                if (genderToDelete == null)
                {
                    return NotFound("Gender with Id ${id} not found.");
                }
                return Ok(await _genderRepository.Delete(id));
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting data");
            }
        }

        // GET: api/Genders/searchKey
        [HttpGet("{searchKey}")]
        public async Task<ActionResult<IEnumerable<Gender>>> Search(string searchKey)
        {
            try
            {
                return Ok(await _genderRepository.Search(d => d.Name.Contains(searchKey)));
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data.");
            }

        }
    }
}
