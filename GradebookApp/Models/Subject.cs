using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradebookApp.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public ICollection<Grade> Grades { get; set; }
    }
}
