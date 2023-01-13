using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.BusinessLogic.Models
{
    internal class Subject
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public int Hours { get; set; }
        public Dictionary<int, string> TeacherSubjects { get; set; }
        public Dictionary<int, string> ClassSubjects { get; set; }
        public List<Lesson> Lessons { get; set; }

    }
}
 