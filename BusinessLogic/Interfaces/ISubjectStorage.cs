using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.BusinessLogic.Interfaces
{
    internal interface ISubjectStorage
    {
        List<ViewModels.Subject> GetFullList();
        List<ViewModels.Subject> GetFilteredList(Models.Subject model);
        ViewModels.Subject GetElement(Models.Subject model);
        void Insert(Models.Subject model);
        void Update(Models.Subject model);
        void Delete(Models.Subject model);
    }
}
