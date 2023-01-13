using CourseWork.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Classroom = CourseWork.BusinessLogic.Models.Classroom;
using VClassroom = CourseWork.BusinessLogic.ViewModels.Classroom;

namespace CourseWork.DBImplement.Implements
{
    internal class ClassroomStorage : IClassroomStorage
    {
        public List<VClassroom> GetFullList()
        {
            using (var context = new ScheduleDB())
            {
                return context.Classrooms
                    .Select(rec => new VClassroom
                    {
                        ID = rec.ID,
                        Name = rec.Name,
                        Capacity = rec.Capacity,
                        Type = rec.Type
                    })
                    .ToList();
            }
        }

        public List<VClassroom> GetFilteredList(Classroom model)
        {
            if (model == null)
            {
                return null;
            }

            using (var context = new ScheduleDB())
            {
                return context.Classrooms
                    .Where(rec => rec.Name.Contains(model.Name))
                    .ToList()
                    .Select(rec => new VClassroom
                    {
                        ID = rec.ID,
                        Name = rec.Name,
                        Capacity = rec.Capacity,
                        Type = rec.Type
                    }).ToList();
            }
        }

        public VClassroom GetElement(Classroom model)
        {
            if (model == null)
            {
                return null;
            }
                
            using (var context = new ScheduleDB())
            {
                Models.Classroom classroom = context.Classrooms
                    .FirstOrDefault(rec => rec.Name == model.Name || rec.ID == model.ID);
                return classroom != null ? new VClassroom
                {
                    ID = classroom.ID,
                    Name = classroom.Name,
                    Capacity = classroom.Capacity,
                    Type = classroom.Type
                } : null;
            }
        }

        public void Insert(Classroom model)
        {
            using (var context = new ScheduleDB())
            {
                context.Classrooms.Add(CreateModel(model, new Models.Classroom()));
                context.SaveChanges();
            }
        }

        public void Update(BusinessLogic.Models.Classroom model)
        {
            using (var context = new ScheduleDB())
            {
                var element = context.Classrooms.FirstOrDefault(rec => rec.ID == model.ID);

                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
                CreateModel(model, element);
                context.SaveChanges();
            }
        }

        public void Delete(BusinessLogic.Models.Classroom model)
        {
            using (var context = new ScheduleDB())
            {
                Models.Classroom element = context.Classrooms.FirstOrDefault(rec => rec.ID == model.ID);

                if (element != null)
                {
                    context.Classrooms.Remove(element);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Элемент не найден");
                }
            }
        }

        private Models.Classroom CreateModel(Classroom model, Models.Classroom classroom)
        {
            classroom.Name = model.Name;
            classroom.Type = model.Type;
            classroom.Capacity = model.Capacity;
            return classroom;
        }
    }
}
