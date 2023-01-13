using CourseWork.BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lesson = CourseWork.BusinessLogic.Models.Lesson;
using VLesson = CourseWork.BusinessLogic.ViewModels.Lesson;

namespace CourseWork.DBImplement.Implements
{
    internal class LessonStorage : ILessonStorage
    {

        public List<VLesson> GetFullList()
        {
            using (var context = new ScheduleDB())
            {
                return context.Lessons
                    .Select(rec => new VLesson
                    {
                        ID = rec.ID,
                        Time = rec.Time,
                        Type = rec.Type,
                        ClassID = rec.ClassID,
                        TeacherID = rec.TeacherID,
                        ClassroomID = rec.ClassroomID,
                        SubjectID = rec.SubjectID
                    })
                    .ToList();
            }
        }

        public VLesson GetElement(Lesson model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new ScheduleDB())
            {
                var lesson = context.Lessons.FirstOrDefault(rec => rec.ID == model.ID);
                return lesson != null ? new VLesson
                {
                    ID = lesson.ID,
                    Time = lesson.Time,
                    Type = lesson.Type,
                    ClassID = lesson.ClassID,
                    TeacherID = lesson.TeacherID,
                    ClassroomID = lesson.ClassroomID,
                    SubjectID = lesson.SubjectID
                } :
                null;
            }
        }

        public List<VLesson> GetFilteredListByClassroom(Lesson model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new ScheduleDB())
            {
                return context.Lessons
                    .Where(rec => (rec.ClassroomID == model.ClassroomID))
                    .ToList().
                    Select(rec => new VLesson
                    {
                        ID = rec.ID,
                        Time = rec.Time,
                        Type = rec.Type,
                        ClassID = rec.ClassID,
                        TeacherID = rec.TeacherID,
                        ClassroomID = rec.ClassroomID,
                        SubjectID = rec.SubjectID,
                    }).ToList();
            }
        }

        public List<VLesson> GetFilteredListByStudent(Lesson model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new ScheduleDB())
            {
                return context.Lessons
                    .Where(rec => (rec.ClassID == model.ClassID))
                    .ToList().
                    Select(rec => new VLesson
                    {
                        ID = rec.ID,
                        Time = rec.Time,
                        Type = rec.Type,
                        ClassID = rec.ClassID,
                        TeacherID = rec.TeacherID,
                        ClassroomID = rec.ClassroomID,
                        SubjectID = rec.SubjectID,
                    }).ToList();
            }
        }

        public List<VLesson> GetFilteredListBySubject(Lesson model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new ScheduleDB())
            {
                return context.Lessons
                    .Where(rec => (rec.SubjectID == model.SubjectID))
                    .ToList().
                    Select(rec => new VLesson
                    {
                        ID = rec.ID,
                        Time = rec.Time,
                        Type = rec.Type,
                        ClassID = rec.ClassID,
                        TeacherID = rec.TeacherID,
                        ClassroomID = rec.ClassroomID,
                        SubjectID = rec.SubjectID,
                    }).ToList();
            }
        }

        public List<VLesson> GetFilteredListByTeacher(Lesson model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new ScheduleDB())
            {
                return context.Lessons
                    .Where(rec => (rec.TeacherID == model.TeacherID))
                    .ToList().
                    Select(rec => new VLesson
                    {
                        ID = rec.ID,
                        Time = rec.Time,
                        Type = rec.Type,
                        ClassID = rec.ClassID,
                        TeacherID = rec.TeacherID,
                        ClassroomID = rec.ClassroomID,
                        SubjectID = rec.SubjectID,
                    }).ToList();
            }
        }

        public void Insert(Lesson model)
        {
            using (var context = new ScheduleDB())
            {
                context.Lessons.Add(CreateModel(model, new Models.Lesson()));
                context.SaveChanges();
            }
        }

        public void Update(Lesson model)
        {
            using (var context = new ScheduleDB())
            {
                var element = context.Lessons.FirstOrDefault(rec => rec.ID == model.ID);

                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
                CreateModel(model, element);
                context.SaveChanges();
            }
        }
        public void Delete(Lesson model)
        {
            using (var context = new ScheduleDB())
            {
                var element = context.Lessons.FirstOrDefault(rec => rec.ID == model.ID);

                if (element != null)
                {
                    context.Lessons.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }
        private Models.Lesson CreateModel(Lesson model, Models.Lesson lesson)
        {
            lesson.Time = model.Time;
            lesson.Type = model.Type;
            lesson.ClassID = (int) model.ClassID;
            lesson.TeacherID = (int) model.TeacherID;
            lesson.ClassroomID = (int) model.ClassroomID;
            lesson.SubjectID = (int) model.SubjectID;
            return lesson;
        }
    }
}
