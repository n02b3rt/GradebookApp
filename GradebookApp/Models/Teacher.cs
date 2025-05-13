using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradebookApp.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<Subject> Subjects { get; set; }

        // Do GUI
        public string FullName => $"{FirstName} {LastName}";
    }

}
