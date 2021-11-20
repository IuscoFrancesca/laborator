using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

public interface IStudentsRepository
{
    Task<List<StudentEntity>> GetAllStudents();
    
    Task<StudentEntity> GetStudent(string id);
    
    Task AddNewStudent(StudentEntity student);
    
    Task EditStudent(StudentEntity student);
    Task DeleteStudent(string id);
    
    
}