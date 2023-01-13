using CourseWork.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CourseWork.BusinessLogic.Logic
{
    internal class LessonLogic
    {
        private readonly ILessonStorage lessonStorage;

        public LessonLogic (ILessonStorage lessonStorage)
        {
            this.lessonStorage = lessonStorage;
        }

        public List<ViewModels.Lesson> Read()
        {
                return lessonStorage.GetFullList();
        }

        public List<ViewModels.Lesson> ReadByTeacherID(Models.Lesson model)
        {
            if (model == null)
            {
                throw new Exception("Элемент не найден");
            }
            return lessonStorage.GetFilteredListByTeacher(model);
        }

        public List<ViewModels.Lesson> ReadByStudentID(Models.Lesson model)
        {
            if (model == null)
            {
                throw new Exception("Элемент не найден");
            }
            return lessonStorage.GetFilteredListByStudent(model);
        }

        public List<ViewModels.Lesson> ReadByClassroomID(Models.Lesson model)
        {
            if (model == null)
            {
                throw new Exception("Элемент не найден");
            }
            return lessonStorage.GetFilteredListByClassroom(model);
        }

        public List<ViewModels.Lesson> ReadBySubjectID(Models.Lesson model)
        {
            if (model == null)
            {
                throw new Exception("Элемент не найден");
            }
            return lessonStorage.GetFilteredListBySubject(model);
        }

        public void CreateOrUpdate(Models.Lesson model)
        {
            var element = lessonStorage.GetElement(new Models.Lesson
            {
                ID = model.ID
            });
            if (element != null)
            {
                throw new Exception("Данная запись уже существует");
            }
            if (model.ID.HasValue)
            {
                lessonStorage.Update(model);
            }
            else
            {
                lessonStorage.Insert(model);
            }
        }
        public void Delete(Models.Lesson model)
        {
            var element = lessonStorage.GetElement(new Models.Lesson { ID = model.ID });
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            lessonStorage.Delete(model);
        }
    }
}
