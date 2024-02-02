using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Data.Repository;
using CollegeApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public DepartmentController(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("All", Name = "GetAllDepartments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartmentsAsync()
        {
            var dep = await _departmentRepository.GetAllAsync();
            var depDtodata = _mapper.Map<List<DepartmentDto>>(dep);

            return Ok(depDtodata);
        }

        [HttpGet("{id:int}", Name ="GetDepartmentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DepartmentDto>> GetByIdAsync(int id)
        {
            if (id == 0)
                return BadRequest();

            var dep = await _departmentRepository.GetAsync(dep => dep.Id == id);
            if (dep == null)
                return NotFound($"The Department id {id} was not found.");

            var depDto = _mapper.Map<DepartmentDto>(dep);

            return Ok(depDto);
        }

        [HttpGet("{name}", Name = "GetDepartmentByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DepartmentDto>> GetDepartmentByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest();

            var dep = await _departmentRepository.GetAsync(dep => dep.DepartmentName == name);
            if (dep == null)
                return NotFound($"The Department with name {name} was not found.");

            var depDto = _mapper.Map<DepartmentDto>(dep);
            return Ok(depDto);
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DepartmentDto>> CreateDepartmentAsync([FromBody]DepartmentDto dto)
        {
            if (dto == null)
                return BadRequest();

            Department department = _mapper.Map<Department>(dto);
            var depAfterCreation = await _departmentRepository.CreateAsync(department);

            dto.Id = depAfterCreation.Id;

            return CreatedAtRoute("GetDepartmentById", new { id = dto.Id }, dto);
        }
    }
}
