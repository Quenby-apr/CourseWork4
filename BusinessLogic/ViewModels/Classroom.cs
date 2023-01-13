using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.BusinessLogic.ViewModels
{
    internal class Classroom
    {
        public int? ID { get; set; }
        [DisplayName("Название")]
        public string Name { get; set; }
        [DisplayName("Вместимость")]
        public int Capacity { get; set; }
        [DisplayName("Тип")]
        public bool Type { get; set; }
        public List<Lesson> Lessons { get; set; }
    }
}
