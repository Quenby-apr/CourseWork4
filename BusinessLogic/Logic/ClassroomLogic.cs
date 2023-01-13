using CourseWork.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.BusinessLogic.Logic
{
    internal class ClassroomLogic
    {
        private readonly IClassroomStorage classroomStorage;

        public ClassroomLogic(IClassroomStorage classroomStorage)
        {
            this.classroomStorage = classroomStorage;
        }

        public List<ViewModels.Classroom> Read(Models.Classroom model)
        {
            if (model == null)
            {
                return classroomStorage.GetFullList();
            }
            if (model.ID.HasValue || model.Name != null)
            {
                return new List<ViewModels.Classroom> { classroomStorage.GetElement(model) };
            }
            return classroomStorage.GetFilteredList(model);
        }

        public void CreateOrUpdate(Models.Classroom model)
        {
            var element = classroomStorage.GetElement(new Models.Classroom
            {
                Name = model.Name
            });
            if (element != null && element.ID != model.ID)
            {
                throw new Exception("Данная запись уже существует");
            }
            if (model.ID.HasValue)
            {
                classroomStorage.Update(model);
            }
            else
            {
                classroomStorage.Insert(model);
            }
        }
        public void Delete(Models.Classroom model)
        {
            var element = classroomStorage.GetElement(new Models.Classroom { ID = model.ID });
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            classroomStorage.Delete(model);
        }
    }
}
