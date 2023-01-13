using CourseWork.DBImplement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.DBImplement
{
    internal class ScheduleDB : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ScheduleDB;Integrated Security=True; MultipleActiveResultSets=True;");
            }
            base.OnConfiguring(optionsBuilder);
        }

        public virtual DbSet<Classroom> Classrooms { set; get; }
        public virtual DbSet<Class> Classes { set; get; }
        public virtual DbSet<Lesson> Lessons { set; get; }
        public virtual DbSet<Subject> Subjects { set; get; }
        public virtual DbSet<Teacher> Teachers { set; get; }
        public virtual DbSet<ClassSubjects> ClassSubjects { set; get; }
        public virtual DbSet<TeacherSubjects> TeacherSubjects { set; get; }
    }
}
