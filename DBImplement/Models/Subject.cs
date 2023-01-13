using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.DBImplement.Models
{
    internal class Subject
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Hours { get; set; }

        [ForeignKey("TeacherID")]
        public List<TeacherSubjects> TeacherSubjects { get; set; }
        [ForeignKey("ClassID")]
        public List<ClassSubjects> ClassSubjects { get; set; }
        [ForeignKey("LessonID")]
        public List<Lesson> Lessons { get; set; }
    }
}
