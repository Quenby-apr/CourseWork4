using CourseWork.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.BusinessLogic.Logic
{
    internal class TeacherLogic
    {
        private readonly ITeacherStorage teacherStorage;

        public TeacherLogic(ITeacherStorage teacherStorage)
        {
            this.teacherStorage = teacherStorage;
        }

        public List<ViewModels.Teacher> Read(Models.Teacher model)
        {
            if (model == null)
            {
                return teacherStorage.GetFullList();
            }
            if (model.ID.HasValue || model.Name != null)
            {
                return new List<ViewModels.Teacher> { teacherStorage.GetElement(model) };
            }
            return teacherStorage.GetFilteredList(model);
        }

        public void CreateOrUpdate(Models.Teacher model)
        {
            var element = teacherStorage.GetElement(new Models.Teacher
            {
                Name = model.Name
            });
            if (element != null && element.ID != model.ID)
            {
                throw new Exception("Данная запись уже существует");
            }
            if (model.ID.HasValue)
            {
                teacherStorage.Update(model);
            }
            else
            {
                teacherStorage.Insert(model);
            }
        }
        public void Delete(Models.Teacher model)
        {
            var element = teacherStorage.GetElement(new Models.Teacher { ID = model.ID });
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            teacherStorage.Delete(model);
        }
    }
}
