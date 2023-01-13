using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.BusinessLogic.Interfaces
{
    internal interface ITeacherStorage
    {
        List<ViewModels.Teacher> GetFullList();
        List<ViewModels.Teacher> GetFilteredList(Models.Teacher model);
        ViewModels.Teacher GetElement(Models.Teacher model);
        void Insert(Models.Teacher model);
        void Update(Models.Teacher model);
        void Delete(Models.Teacher model);
    }
}
