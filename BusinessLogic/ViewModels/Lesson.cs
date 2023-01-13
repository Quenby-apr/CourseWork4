using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.BusinessLogic.ViewModels
{
    internal class Lesson
    {
        public int? ID { get; set; }
        [DisplayName("Время начала урока")]
        public int Time { get; set; }
        [DisplayName("Тип")]
        public bool Type { get; set; }
        public int ClassID { get; set; }
        public int TeacherID { get; set; }
        public int ClassroomID { get; set; }
        public int SubjectID { get; set; }
    }
}
