using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MonitoringStudents.Models;

namespace MonitoringStudents.Repositories
{
    public static class StudentsRepo
    {
        public static List<Student> Students = new List<Student>()
        {
            new Student(){ ID = 1, FirstName = "Francesca", Year = 4, Faculty = "AC" },
            new Student(){ ID = 2, FirstName = "Corina", Year = 4, Faculty = "AC" },
            new Student(){ ID = 3, FirstName = "Darius", Year = 4, Faculty = "ETC"}

        };
    }
}
