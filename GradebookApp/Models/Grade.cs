using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradebookApp.Models
{
    public class Grade
    {
        public int GradeId { get; set; }
        public double Value { get; set; }
        public DateTime Date { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public string Comment { get; set; }
    }
}
