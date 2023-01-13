using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.DBImplement.Models
{
    internal class TeacherSubjects
    {
        public int ID { get; set; }
        public int TeacherID { get; set; }
        public int SubjectID { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
