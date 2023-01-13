using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.BusinessLogic.ViewModels
{
    internal class Teacher
    {
        public int? ID { get; set; }
        [DisplayName("Имя")]
        public string Name { get; set; }
        [DisplayName("Приоритет")]
        public int Priority { get; set; }
        [DisplayName("Начальный урок")]
        public int StartDay { get; set; }
        [DisplayName("Заключительный урок")]
        public int EndDay { get; set; }
        public int Hours { get; set; }
        public List<int> WorkDays { get; set; }
        public Dictionary<int, string> TeacherSubjects { get; set; }
        public List<Lesson> Lessons { get; set; }
    }
}
