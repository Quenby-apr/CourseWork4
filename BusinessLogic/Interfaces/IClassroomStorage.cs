using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.BusinessLogic.Interfaces
{
    internal interface IClassroomStorage
    {
        List<ViewModels.Classroom> GetFullList();
        List<ViewModels.Classroom> GetFilteredList(Models.Classroom model);
        ViewModels.Classroom GetElement(Models.Classroom model);
        void Insert(Models.Classroom model);
        void Update(Models.Classroom model);
        void Delete(Models.Classroom model);
    }
}
