using CourseWork.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.BusinessLogic.Logic
{
    internal class ClassLogic
    {
        private readonly IClassStorage classStorage;

        public ClassLogic(IClassStorage classStorage)
        {
            this.classStorage = classStorage;
        }

        public List<ViewModels.Class> Read(Models.Class model)
        {
            if (model == null)
            {
                return classStorage.GetFullList();
            }
            if (model.ID.HasValue || model.Name != null)
            {
                return new List<ViewModels.Class> { classStorage.GetElement(model) };
            }
            return classStorage.GetFilteredList(model);
        }

        public void CreateOrUpdate(Models.Class model)
        {
            var element = classStorage.GetElement(new Models.Class
            {
                Name = model.Name
            });
            if (element != null && element.ID != model.ID)
            {
                throw new Exception("Данная запись уже существует");
            }
            if (model.ID.HasValue)
            {
                classStorage.Update(model);
            }
            else
            {
                classStorage.Insert(model);
            }
        }
        public void Delete(Models.Class model)
        {
            var element = classStorage.GetElement(new Models.Class { ID = model.ID });
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            classStorage.Delete(model);
        }
    }
}
