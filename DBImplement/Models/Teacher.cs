using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.DBImplement.Models
{
    internal class Teacher
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public int Priority { get; set; }
        public int StartDay { get; set; }
        public int EndDay { get; set; }
        [Required]
        public int Hours { get; set; }
        [Required]
        public List<int> WorkDays { get; set; }
        [ForeignKey("SubjectID")]
        public List<TeacherSubjects> TeacherSubjects { get; set; }
        [ForeignKey("LessonID")]
        public List<Lesson> Lessons { get; set; }
    }
}
