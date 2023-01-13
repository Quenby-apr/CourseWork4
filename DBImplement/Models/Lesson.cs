using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.DBImplement.Models
{
    internal class Lesson
    {
        public int ID { get; set; }
        [Required]
        public int Time { get; set; }
        [Required]
        public bool Type { get; set; }
        public int ClassID { get; set; }
        public virtual Class Class { get; set; }
        public int TeacherID { get; set; }
        public virtual Teacher Teacher { get; set; }
        public int ClassroomID { get; set; }
        public virtual Classroom Classroom { get; set; }
        public int SubjectID { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
