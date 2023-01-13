using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.DBImplement.Models
{
    internal class ClassSubjects
    {
        public int ID { get; set; }
        public int ClassID { get; set; }
        public int SubjectID { get; set; }
        public virtual Class Class { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
