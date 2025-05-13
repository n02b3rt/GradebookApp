using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradebookApp.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StudentNumber { get; set; }

        public int ClassId { get; set; }
        public Class Class { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<Grade> Grades { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
