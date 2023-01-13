using CourseWork.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.BusinessLogic.Logic
{
    internal class SubjectLogic
    {
        private readonly ISubjectStorage subjectStorage;

        public SubjectLogic(ISubjectStorage subjectStorage)
        {
            this.subjectStorage = subjectStorage;
        }

        public List<ViewModels.Subject> Read(Models.Subject model)
        {
            if (model == null)
            {
                return subjectStorage.GetFullList();
            }
            if (model.ID.HasValue || model.Name != null)
            {
                return new List<ViewModels.Subject> { subjectStorage.GetElement(model) };
            }
            return subjectStorage.GetFilteredList(model);
        }

        public void CreateOrUpdate(Models.Subject model)
        {
            var element = subjectStorage.GetElement(new Models.Subject
            {
                Name = model.Name
            });
            if (element != null && element.ID != model.ID)
            {
                throw new Exception("Данная запись уже существует");
            }
            if (model.ID.HasValue)
            {
                subjectStorage.Update(model);
            }
            else
            {
                subjectStorage.Insert(model);
            }
        }
        public void Delete(Models.Subject model)
        {
            var element = subjectStorage.GetElement(new Models.Subject { ID = model.ID });
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            subjectStorage.Delete(model);
        }
    }
}
