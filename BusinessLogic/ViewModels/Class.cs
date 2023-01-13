using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.BusinessLogic.ViewModels
{
    internal class Class
    {
        public int? ID { get; set; }
        [DisplayName("Название")]
        public string Name { get; set; }
        [DisplayName("Часы нагрузки")]
        public int Hours { get; set; }
        [DisplayName("Курс")]
        public int Course { get; set; }
        [DisplayName("Количество студентов")]
        public int CountStudents { get; set; }
        [DisplayName("Оценка")]
        public int Estimate { get; set; }
        public Dictionary<int, string> ClassSubjects { get; set; }
        public List<Lesson> Lessons { get; set; }
    }
}
