using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.BusinessLogic.Models
{
    internal class Classroom
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public bool Type { get; set; }
        public List<Lesson> Lessons { get; set; }
    }
}
