using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
using MonitoringStudents.Models;
using MonitoringStudents.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
//using System.Threading.Tasks;

namespace MonitoringStudents.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        public StudentsController() { }

        [HttpGet]
        public IEnumerable<Student> Get()
        {
            return StudentsRepo.Students;
        }

        [HttpGet("{id}")]
        public Student Get(int id)
        {
            return StudentsRepo.Students.FirstOrDefault(s => s.ID == id);
        }

        [HttpPost]
        public void Post([FromBody] Student student)
        {
            StudentsRepo.Students.Add(student);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            StudentsRepo.Students.Remove(StudentsRepo.Students.FirstOrDefault(s => s.ID == id));
        }

        [HttpPut]
        public void Put(int id, [FromBody] Student student)
        {
            var StudentChange = StudentsRepo.Students.FirstOrDefault(s => s.ID == id);
            StudentChange.FirstName = student.FirstName;
            StudentChange.Year = student.Year;
            StudentChange.Faculty = student.Faculty;
            /*if(StudentChange.ID==student.ID)
            {
                StudentChange.FirstName = student.FirstName;
                StudentChange.Year = student.Year;
                StudentChange.Faculty = student.Faculty;
            }
            else
            {
                NotFound();
            }*/

        }
    }
}
