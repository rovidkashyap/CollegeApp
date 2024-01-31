using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Data.Repository;
using CollegeApp.Models;
using CollegeApp.MyLogging;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IMyLogger _myLogger;
        //private readonly ICollegeRepository<Student> _studentRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        public StudentController(IStudentRepository studentRepository, IMyLogger logger, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _myLogger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("All", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudentsAsync()
        {
            _myLogger.Log("This is Logger Message");

            //var students = await _context.Students.ToListAsync();
            var students = await _studentRepository.GetAllAsync();
            var studentDtoData = _mapper.Map<List<StudentDto>>(students);

            //var students = await _context.Students.Select(s => new StudentDto()
            //{
            //    Id = s.Id,
            //    StudentName = s.StudentName,
            //    Address = s.Address,
            //    Email = s.Email,
            //    DOB = s.DOB
            //}).ToListAsync();
            return Ok(students);
        }
        
        [HttpGet("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDto>> GetStudentByIdAsync(int id)
        {
            // ERROR 400 - BadRequest - Client Error
            if (id <= 0)
                return BadRequest();

            //var student = await _context.Students.Where(x => x.Id == id).FirstOrDefaultAsync();
            var student = await _studentRepository.GetAsync(student => student.Id == id);
            // ERROR 404 - NotFound - Client Error 
            if (student == null)
                return NotFound($"The Student with id {id} not found.");

            var studentDto = _mapper.Map<StudentDto>(student);

            //var studentDto = new StudentDto()
            //{
            //    Id = student.Id,
            //    StudentName = student.StudentName,
            //    Address = student.Address,
            //    Email = student.Email,
            //    DOB = student.DOB
            //};

            // 200 - OK Success
            return Ok(studentDto);
        }

        [HttpGet("{name}", Name = "GetStudentByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDto>> GetStudentByNameAsync(string name)
        {
            // ERROR 400 - BadRequest - Client Error
            if (string.IsNullOrEmpty(name))
                return BadRequest();

            //var student = await _context.Students.Where(x => x.StudentName == name).FirstOrDefaultAsync();
            var student = await _studentRepository.GetAsync(student => student.StudentName.ToLower().Contains(name.ToLower()));
            // ERROR 404 - NotFound - Client Error
            if (student == null)
                return NotFound($"The Student with name {name} not found.");

            var studentDto = _mapper.Map<StudentDto>(student);

            //var studentDto = new StudentDto()
            //{
            //    Id = student.Id,
            //    StudentName = student.StudentName,
            //    Address = student.Address,
            //    Email = student.Email,
            //    DOB = student.DOB
            //};

            // 200 - OK Success
            return Ok(studentDto);
        }

        [HttpPost]
        [Route("Create")]
        //api/Student/create
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDto>> CreateStudentAsync([FromBody]StudentDto dto)
        {
            if (dto == null)
                return BadRequest();

            //int newId = _context.Students.LastOrDefault().Id + 1;

            //Student student = new Student
            //{
            //    StudentName = model.StudentName,
            //    Address = model.Address,
            //    Email = model.Email,
            //    DOB = model.DOB
            //};

            Student student = _mapper.Map<Student>(dto);
            var studentAfterCreation = await _studentRepository.CreateAsync(student);
            //await _context.Students.AddAsync(student);
            //await _context.SaveChangesAsync();

            dto.Id = studentAfterCreation.Id;

            // Status Code - 201 Created
            return CreatedAtRoute("GetStudentById", new { id = dto.Id }, dto);
            //return Ok(model);
        }

        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDto>> UpdateStudentAsync([FromBody]StudentDto dto)
        {
            if (dto == null || dto.Id > 0)
                BadRequest();

            //var existingStudent = await _context.Students.Where(s => s.Id == dto.Id).FirstOrDefaultAsync();
            var existingStudent = await _studentRepository.GetAsync(student => student.Id == dto.Id);

            if (existingStudent == null)
                return NotFound();

            //existingStudent.StudentName = model.StudentName;
            //existingStudent.Address = model.Address;
            //existingStudent.Email = model.Email;
            //existingStudent.DOB = model.DOB;

            var newRecord = _mapper.Map<Student>(dto);
            await _studentRepository.UpdateAsync(newRecord);
            //_context.Students.Update(newRecord);
            //await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch]
        [Route("{id:int}/UpdatePartial")]
        //api/student/1/updatepartial
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDto>> UpdateStudentPartialAsync(int id, [FromBody] JsonPatchDocument<StudentDto> patchDocument)
        {
            if (patchDocument == null || id > 0)
                BadRequest();

            //var existingStudent = await _context.Students.Where(s => s.Id == id).FirstOrDefaultAsync();
            var existingStudent = await _studentRepository.GetAsync(student => student.Id == id);

            if (existingStudent == null)
                return NotFound();

            //var studentDto = new StudentDto
            //{
            //    Id = existingStudent.Id,
            //    StudentName = existingStudent.StudentName,
            //    Email = existingStudent.Email,
            //    Address = existingStudent.Address,
            //    DOB = existingStudent.DOB
            //};

            var studentDto = _mapper.Map<StudentDto>(existingStudent);

            patchDocument.ApplyTo(studentDto, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            existingStudent = _mapper.Map<Student>(studentDto);

            //existingStudent.StudentName = studentDto.StudentName;
            //existingStudent.Email = studentDto.Email;
            //existingStudent.Address = studentDto.Address;
            //existingStudent.DOB = studentDto.DOB;
            //await _context.SaveChangesAsync();

            await _studentRepository.UpdateAsync(existingStudent);
            
            //204 - No Content
            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteStudentAsync(int id)
        {
            // ERROR 400 - BadRequest - Client Error
            if (id <= 0)
                return BadRequest();

            //var student = await _context.Students.Where(s => s.Id == id).FirstOrDefaultAsync();
            var student = await _studentRepository.GetAsync(student => student.Id == id);

            // ERROR 404 - NotFound - Client Error
            if (student == null)
                return NotFound();

            await _studentRepository.DeleteAsync(student);

            //_context.Students.Remove(student);
            //await _context.SaveChangesAsync();

            // 200 - OK Success
            return Ok(true);
        }
    }
}
