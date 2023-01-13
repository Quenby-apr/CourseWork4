using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.DBImplement.Models
{
    internal class Class
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Hours { get; set; }
        [Required]
        public int Course { get; set; }
        [Required]
        public int CountStudents { get; set; }
        public int Estimate { get; set; }
        [ForeignKey("SubjectID")]
        public virtual List<ClassSubjects> ClassSubjects { get; set; }
        [ForeignKey("LessonID")]
        public virtual List<Lesson> Lessons { get; set; }
    }
}
