using CourseWork.BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teacher = CourseWork.BusinessLogic.Models.Teacher;
using VTeacher = CourseWork.BusinessLogic.ViewModels.Teacher;

namespace CourseWork.DBImplement.Implements
{
    internal class TeacherStorage : ITeacherStorage
    {
        public List<VTeacher> GetFullList()
        {
            using (var context = new ScheduleDB())
            {
                return context.Teachers
                    .Include(rec => rec.TeacherSubjects)
                    .ThenInclude(rec => rec.Subject).ToList()
                    .Select(rec => new VTeacher
                    {
                        ID = rec.ID,
                        Name = rec.Name,
                        Priority = rec.Priority,
                        StartDay = rec.StartDay,
                        EndDay = rec.EndDay,
                        Hours = rec.Hours,
                        WorkDays = rec.WorkDays,
                        TeacherSubjects = rec.TeacherSubjects
                        .ToDictionary(recTS => recTS.SubjectID, recTS => (recTS.Subject?.Name))
                    })
                    .ToList();
            }
        }

        public VTeacher GetElement(Teacher model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new ScheduleDB())
            {
                var teacher = context.Teachers
                  .Include(rec => rec.TeacherSubjects)
                  .ThenInclude(rec => rec.Subject)
                  .FirstOrDefault(rec => rec.ID == model.ID);
                return teacher != null ? new VTeacher
                {
                    ID = teacher.ID,
                    Name = teacher.Name,
                    Priority = teacher.Priority,
                    StartDay = teacher.StartDay,
                    EndDay = teacher.EndDay,
                    Hours = teacher.Hours,
                    WorkDays = teacher.WorkDays,
                    TeacherSubjects = teacher.TeacherSubjects
                        .ToDictionary(recTS => recTS.SubjectID, recTS => (recTS.Subject?.Name))
                } :
                null;
            }
        }

        public List<VTeacher> GetFilteredList(Teacher model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new ScheduleDB())
            {
                return context.Teachers
                    .Include(rec => rec.TeacherSubjects)
                    .ThenInclude(rec => rec.Subject).ToList()
                    .Select(rec => new VTeacher
                    {
                        ID = rec.ID,
                        Name = rec.Name,
                        Priority = rec.Priority,
                        StartDay = rec.StartDay,
                        EndDay = rec.EndDay,
                        Hours = rec.Hours,
                        WorkDays = rec.WorkDays,
                        TeacherSubjects = rec.TeacherSubjects
                        .ToDictionary(recTS => recTS.SubjectID, recTS => (recTS.Subject?.Name))
                    })
                    .ToList();
            }
        }

        public void Insert(Teacher model)
        {
            using (var context = new ScheduleDB())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Models.Teacher teacher = new Models.Teacher
                        {
                            Name = model.Name,
                            Priority = model.Priority,
                            StartDay = model.StartDay,
                            EndDay = model.EndDay,
                            Hours = model.Hours,
                            WorkDays = model.WorkDays,
                        };
                        context.Teachers.Add(teacher);
                        context.SaveChanges();
                        CreateModel(model, teacher, context); ;
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Update(Teacher model)
        {
            using (var context = new ScheduleDB())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var element = context.Teachers.FirstOrDefault(rec => rec.ID ==
                       model.ID);
                        if (element == null)
                        {
                            throw new Exception("Элемент не найден");
                        }
                        CreateModel(model, element, context);
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public void Delete(Teacher model)
        {
            using (var context = new ScheduleDB())
            {
                Models.Teacher element = context.Teachers.FirstOrDefault(rec => rec.ID ==
               model.ID);
                if (element != null)
                {
                    context.Teachers.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }

        private Models.Teacher CreateModel(Teacher model, Models.Teacher teacher, ScheduleDB context)
        {
            teacher.Name = model.Name;
            teacher.Priority = model.Priority;
            teacher.StartDay = model.StartDay;
            teacher.EndDay = model.EndDay;
            teacher.Hours = model.Hours;
            teacher.WorkDays = model.WorkDays;
            if (model.TeacherSubjects != null)
            {
                if (model.ID.HasValue)
                {
                    var teacherSubjects = context.TeacherSubjects.Where(rec =>
                   rec.TeacherID == model.ID.Value).ToList();
                    context.TeacherSubjects.RemoveRange(teacherSubjects.Where(rec =>
                   !model.TeacherSubjects.ContainsKey(rec.SubjectID)).ToList());
                    context.SaveChanges();
                }
                foreach (var cs in model.TeacherSubjects)
                {
                    context.TeacherSubjects.Add(new Models.TeacherSubjects
                    {
                        TeacherID = teacher.ID,
                        SubjectID = cs.Key,
                    });
                    context.SaveChanges();
                }
            }
            return teacher;
        }
    }
}
