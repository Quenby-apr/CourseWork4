using CourseWork.BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Subject = CourseWork.BusinessLogic.Models.Subject;
using VSubject = CourseWork.BusinessLogic.ViewModels.Subject;

namespace CourseWork.DBImplement.Implements
{
    internal class SubjectStorage : ISubjectStorage
    {
        public VSubject GetElement(Subject model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new ScheduleDB())
            {
                var subject = context.Subjects
                    .Include(rec => rec.TeacherSubjects)
                    .ThenInclude(rec => rec.Teacher)
                  .FirstOrDefault(rec => rec.ID == model.ID);
                return subject != null ? new VSubject
                {
                    ID = subject.ID,
                    Name = subject.Name,
                    Hours = subject.Hours,
                    TeacherSubjects = subject.TeacherSubjects
                        .ToDictionary(recTS => recTS.TeacherID, recTS => (recTS.Teacher.Name))
                } :
                null;
            }
        }

        public List<VSubject> GetFilteredList(Subject model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new ScheduleDB())
            {
                return context.Subjects
                    .Select(rec => new VSubject
                    {
                        ID = rec.ID,
                        Name = rec.Name,
                        Hours = rec.Hours
                    }).ToList();
            }
        }

        public List<VSubject> GetFullList()
        {
            using (var context = new ScheduleDB())
            {
                return context.Subjects
                    .Include(rec => rec.ClassSubjects)
                    .ThenInclude(rec => rec.Class)
                    .Include(rec => rec.TeacherSubjects)
                    .ThenInclude(rec => rec.Teacher)
                    .ToList()
                    .Select(rec => new VSubject
                    {
                        ID = rec.ID,
                        Name = rec.Name,
                        Hours = rec.Hours,
                        ClassSubjects = rec.ClassSubjects
                        .ToDictionary(recCS => recCS.ClassID, recCS => (recCS.Class.Name)),
                        TeacherSubjects = rec.TeacherSubjects
                        .ToDictionary(recTS => recTS.TeacherID, recTS => (recTS.Teacher.Name))
                    })
                    .ToList();
            }
        }

        public void Insert(Subject model)
        {
            using (var context = new ScheduleDB())
            {
                context.Subjects.Add(CreateModel(model, new Models.Subject()));
                context.SaveChanges();
            }
        }

        public void Update(Subject model)
        {
            using (var context = new ScheduleDB())
            {
                var element = context.Subjects.FirstOrDefault(rec => rec.ID == model.ID);

                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
                CreateModel(model, element);
                context.SaveChanges();
            }
        }
        public void Delete(Subject model)
        {
            using (var context = new ScheduleDB())
            {
                var element = context.Subjects.FirstOrDefault(rec => rec.ID == model.ID);

                if (element != null)
                {
                    context.Subjects.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }

        private Models.Subject CreateModel(Subject model, Models.Subject subject)
        {
            subject.ID = (int) model.ID;
            subject.Name = model.Name;
            subject.Hours = model.Hours;
            return subject;
        }
    }
}
