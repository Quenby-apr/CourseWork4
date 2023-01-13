using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.BusinessLogic.Models
{
    internal class Teacher
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public int StartDay { get; set; }
        public int EndDay { get; set; }
        public int Hours { get; set; }
        public List<int> WorkDays { get; set; }
        public Dictionary<int, string> TeacherSubjects { get; set; }
        public List<Lesson> Lessons { get; set; }
    }
}
