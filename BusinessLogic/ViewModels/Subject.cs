using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.BusinessLogic.ViewModels
{
    internal class Subject
    {
        public int? ID { get; set; }
        [DisplayName("Название")]
        public string Name { get; set; }
        [DisplayName("Часы")]
        public int Hours { get; set; }
        public Dictionary<int, string> TeacherSubjects { get; set; }
        public Dictionary<int, string> ClassSubjects { get; set; }
        public List<Lesson> Lessons { get; set; }

    }
}
