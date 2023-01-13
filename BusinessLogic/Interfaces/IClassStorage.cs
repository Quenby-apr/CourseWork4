using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.BusinessLogic.Interfaces
{
    internal interface IClassStorage
    {
        List<ViewModels.Class> GetFullList();
        List<ViewModels.Class> GetFilteredList(Models.Class model);
        ViewModels.Class GetElement(Models.Class model);
        void Insert(Models.Class model);
        void Update(Models.Class model);
        void Delete(Models.Class model);
    }
}
