using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.BusinessLogic.Models
{
    internal class Lesson
    {
        public int? ID { get; set; }
        public int Time { get; set; }
        public bool Type { get; set; }
        public int? ClassID { get; set; }
        public int? TeacherID { get; set; }
        public int? ClassroomID { get; set; }
        public int? SubjectID { get; set; }
    }
}
