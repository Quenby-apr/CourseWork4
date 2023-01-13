using CourseWork.BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Class = CourseWork.BusinessLogic.Models.Class;
using VClass = CourseWork.BusinessLogic.ViewModels.Class;


namespace CourseWork.DBImplement.Implements
{
    internal class ClassStorage : IClassStorage
    {
        public List<VClass> GetFullList()
        {
            using (var context = new ScheduleDB())
            {
                return context.Classes
                    .Include(rec => rec.ClassSubjects)
                    .ThenInclude(rec => rec.Subject).ToList()
                    .Select(rec => new VClass
                    {
                        ID = rec.ID,
                        Name = rec.Name,
                        Hours = rec.Hours,
                        Course = rec.Course,
                        CountStudents = rec.CountStudents,
                        Estimate = rec.Estimate,
                        ClassSubjects = rec.ClassSubjects
                        .ToDictionary(recCS => recCS.SubjectID, recCS => (recCS.Subject?.Name))
                    })
                    .ToList();
            }
        }

        public List<VClass> GetFilteredList(Class model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new ScheduleDB())
            {
                return context.Classes
                    .Include(rec => rec.ClassSubjects)
                    .ThenInclude(rec => rec.Subject).ToList()
                    .Select(rec => new VClass
                    {
                        ID = rec.ID,
                        Name = rec.Name,
                        Hours = rec.Hours,
                        Course = rec.Course,
                        CountStudents = rec.CountStudents,
                        Estimate = rec.Estimate,
                        ClassSubjects = rec.ClassSubjects
                        .ToDictionary(recCS => recCS.SubjectID, recCS => (recCS.Subject?.Name))
                    })
                    .ToList();
            }
        }

        public VClass GetElement(Class model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new ScheduleDB())
            {
                var clazz = context.Classes
                  .Include(rec => rec.ClassSubjects)
                  .ThenInclude(rec => rec.Subject)
                  .FirstOrDefault(rec => rec.ID == model.ID);
                return clazz != null ? new VClass
                {
                    ID = clazz.ID,
                    Name = clazz.Name,
                    Hours = clazz.Hours,
                    Course = clazz.Course,
                    CountStudents = clazz.CountStudents,
                    Estimate = clazz.Estimate,
                    ClassSubjects = clazz.ClassSubjects
                        .ToDictionary(recCS => recCS.SubjectID, recCS => (recCS.Subject?.Name))
                } :
                null;
            }
        }

        public void Insert(Class model)
        {
            using (var context = new ScheduleDB())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Models.Class clazz = new Models.Class
                        {
                            Name = model.Name,
                            Course = model.Course,
                            Hours = model.Hours,
                            CountStudents = model.CountStudents,
                            Estimate = model.Estimate
                        };
                        context.Classes.Add(clazz);
                        context.SaveChanges();
                        CreateModel(model, clazz, context);;
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

        public void Update(Class model)
        {
            using (var context = new ScheduleDB())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var element = context.Classes.FirstOrDefault(rec => rec.ID ==
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
        public void Delete(Class model)
        {
            using (var context = new ScheduleDB())
            {
                Models.Class element = context.Classes.FirstOrDefault(rec => rec.ID ==
               model.ID);
                if (element != null)
                {
                    context.Classes.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }

        private Models.Class CreateModel(Class model, Models.Class clazz, ScheduleDB context)
        {
            clazz.Name = model.Name;
            clazz.Course = model.Course;
            clazz.Hours = model.Hours;
            clazz.CountStudents = model.CountStudents;
            clazz.Estimate = model.Estimate;
            if (model.ClassSubjects != null)
            {
                if (model.ID.HasValue)
                {
                    var classSubjects = context.ClassSubjects.Where(rec =>
                   rec.ClassID == model.ID.Value).ToList();
                    context.ClassSubjects.RemoveRange(classSubjects.Where(rec =>
                   !model.ClassSubjects.ContainsKey(rec.SubjectID)).ToList());
                    context.SaveChanges();
                }
                foreach (var cs in model.ClassSubjects)
                {
                    context.ClassSubjects.Add(new Models.ClassSubjects
                    {
                        ClassID = clazz.ID,
                        SubjectID = cs.Key,
                    });
                    context.SaveChanges();
                }
            }
            return clazz;
        }
    }
}
