using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradebookApp.Models
{
    public class Class
    {
        public int ClassId { get; set; }
        public string Name { get; set; }

        public ICollection<Student> Students { get; set; }
    }
}
