using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.BusinessLogic.Interfaces
{
    internal interface ILessonStorage
    {
        List<ViewModels.Lesson> GetFullList();
        List<ViewModels.Lesson> GetFilteredListByTeacher(Models.Lesson model);
        List<ViewModels.Lesson> GetFilteredListByStudent(Models.Lesson model);
        List<ViewModels.Lesson> GetFilteredListByClassroom(Models.Lesson model);
        List<ViewModels.Lesson> GetFilteredListBySubject(Models.Lesson model);
        ViewModels.Lesson GetElement(Models.Lesson model);
        void Insert(Models.Lesson model);
        void Update(Models.Lesson model);
        void Delete(Models.Lesson model);
    }
}
