using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace L06WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        private IStudentsRepository _studentsRepository;
        public StudentsController(IStudentsRepository studentsRepository)
        {
            _studentsRepository=studentsRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<StudentEntity>> Get()
        {
            return await _studentsRepository.GetAllStudents();
        }

        [HttpGet("{id}")]
        public async Task<StudentEntity> GetStudent([FromRoute] string id)
        {
            return await _studentsRepository.GetStudent(id);
        }

        [HttpPost]

        public async Task<string> AddStudent([FromBody] StudentEntity student)
        {
            try
            {
                await _studentsRepository.AddNewStudent(student);
                return "Studentul a fost adaugat cu succes!";
            }
            catch(System.Exception e)
            {
                return e.Message;
            }
        }

        [HttpDelete("{id}")]
        public async Task<string> Delete([FromRoute] string id)
        {
            try
            {
                await _studentsRepository.DeleteStudent(id);

                return "Student sters cu succes!";
            }
            catch(System.Exception e)
            {
                return e.Message;
            }
        }

        [HttpPut]
        public async Task<string> Edit([FromBody] StudentEntity student)
        {
            try
            {
                await _studentsRepository.EditStudent(student);
                return "Student modificat cu succes!";
            }
            catch(System.Exception e)
            {
                return e.Message;
            }
        }



    }
}