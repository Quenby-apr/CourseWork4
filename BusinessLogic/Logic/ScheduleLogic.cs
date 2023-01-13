using CourseWork.BusinessLogic.Interfaces;
using CourseWork.DBImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lesson = CourseWork.BusinessLogic.Models.Lesson;
using VLesson = CourseWork.BusinessLogic.ViewModels.Lesson;
using VTeacher = CourseWork.BusinessLogic.ViewModels.Teacher;
using VClass = CourseWork.BusinessLogic.ViewModels.Class;
using VSubject = CourseWork.BusinessLogic.ViewModels.Subject;

namespace CourseWork.BusinessLogic.Logic
{
    internal class ScheduleLogic
    {
        private readonly ITeacherStorage teacherStorage;
        private readonly IClassStorage classStorage;
        private readonly IClassroomStorage classroomStorage;
        private readonly ISubjectStorage subjectStorage;
        Dictionary<int, Dictionary<int, int[]>> distributionTeachers;
        Dictionary<int, Dictionary<int, int[]>> distributionClasses;
        Dictionary<int, Dictionary<int, int[]>> teachersClasses;
        Dictionary<int, double> personEstimates;
        private int countHoursInDay;
        private int countWorkDays;
        private bool lowMutationTeacher;
        private int countDifferentMutationType;
        private int countSameMutationPerson;
        private int teacherCoefficient;
        private int classCoefficient;
        private int survivingPersons;
        public ScheduleLogic(ITeacherStorage teacherStorage, IClassStorage classStorage, IClassroomStorage classroomStorage, ISubjectStorage subjectStorage)
        {
            this.teacherStorage = teacherStorage;
            this.classStorage = classStorage;
            this.classroomStorage = classroomStorage;
            this.subjectStorage = subjectStorage;
            personEstimates = new Dictionary<int, double>();
            countHoursInDay = 8;
            countWorkDays = 6;
            lowMutationTeacher = true;
            countDifferentMutationType = 5;
            countSameMutationPerson = 8;
            teacherCoefficient = 1;
            classCoefficient = 1;
            survivingPersons = 3;
    }

        public List<VLesson> GenereateSchedule()
        {
            StartGenerate();
            return GeneticAlgorithm();
        }

        private List<VLesson> GeneticAlgorithm()
        {
            Reproduction(distributionTeachers, distributionClasses, teachersClasses); //размножение и мутация
            FitnessFunction(); // оценка и отсев
            // по окончании цикла получения списка уроков
            return null;
        }

        private void StartGenerate()
        {
            Dictionary<int, int[]> distributionTeachersPerson = new Dictionary<int, int[]>(); // предметы + учителя
            Dictionary<int, int[]> distributionClassesPerson = InitClasses(); // предметы + ученики
            Dictionary<int, int[]> teachersClassesPerson = new Dictionary<int, int[]>(); // учителя + ученики
            List <VTeacher> teacherList = teacherStorage.GetFullList();
            List <VSubject> subjectList = subjectStorage.GetFullList();
            foreach (var teacher in teacherList) // проходим по каждому учителю в списке
            {
                int[] distributionTeacher = InitTeacher(teacher);
                int[] teacherClasses = new int[countHoursInDay * countWorkDays];
                int remainingTeacherHours = teacher.Hours; 
                foreach (var subject in teacher.TeacherSubjects) // просматриваем предметы, которые ведёт учитель
                {
                    bool skip = false; //флаг перехода к следующему учителю
                    foreach (var subj in subjectList) 
                    {
                        if (subj.Hours > remainingTeacherHours) //если предмет для группы занимает больше времени оставшихся часов, то переход к следующему учителю  
                        {
                            skip = true;
                            break;
                        }
                        if (subj.Name == subject.Value)
                        {
                            for (int iter = 0; iter < subj.ClassSubjects.Count; iter++) //распределение преподавателя на разные классы
                            {
                                var clazz = subj.ClassSubjects.First(); // берём первую попавшуюся группу
                                for (int i = 0; i < subj.Hours; i++) // заполняем планы преподавателя и группы данным предметом
                                {
                                    int spentHours = teacher.Hours - remainingTeacherHours;
                                    int dayNumber = spentHours + 1 / (teacher.EndDay - teacher.StartDay + 1);
                                    int lessonTime = spentHours + 1 % (teacher.EndDay - teacher.StartDay + 1);
                                    int indexLesson = countHoursInDay * teacher.WorkDays[dayNumber] + lessonTime;
                                    distributionClassesPerson[clazz.Key][indexLesson] = ((int)(subj.ID + 1)); // заполняем информацию о группе, которую взял преподаватель, деление на 6 из-за 6-дневки
                                    distributionTeacher[indexLesson] = ((int)(subj.ID + 1));
                                    remainingTeacherHours--;
                                    teacherClasses[indexLesson] = clazz.Key;
                                }
                                subj.ClassSubjects.Remove(clazz.Key); // удаляем информацию о классе, чтоб не было дублей в цикле
                            }
                        }
                    }
                    if (skip)
                    {
                        break;
                    }
                }
                teachersClassesPerson.Add((int)teacher.ID, teacherClasses);
                distributionTeachersPerson.Add((int)teacher.ID, distributionTeacher);
            }
            distributionTeachers.Add(0, distributionTeachersPerson);
            distributionClasses.Add(0, distributionClassesPerson);
            teachersClasses.Add(0, teachersClassesPerson);
        }


        private void Reproduction(Dictionary<int, Dictionary<int, int[]>> distributionTeachers, Dictionary<int, Dictionary<int, int[]>> distributionClasses, Dictionary<int, Dictionary<int, int[]>> teachersClasses)
        {
            Dictionary<int, Dictionary<int, int[]>> _distributionTeachers = new Dictionary<int, Dictionary<int, int[]>>(distributionTeachers);
            Dictionary<int, Dictionary<int, int[]>> _distributionClasses = new Dictionary<int, Dictionary<int, int[]>>(distributionClasses);
            Dictionary<int, Dictionary<int, int[]>> _teachersClasses = new Dictionary<int, Dictionary<int, int[]>>(teachersClasses);
            foreach (var person in distributionTeachers)
            {
                for (int i = 0; i < countDifferentMutationType; i++)
                {
                    for (int j = 0; j < countSameMutationPerson; j++)
                    {
                        Dictionary<int, int[]> distributionTeachersPerson = new Dictionary<int, int[]>(distributionTeachers[person.Key]); // предметы + учителя
                        Dictionary<int, int[]> distributionClassesPerson = new Dictionary<int, int[]>(distributionClasses[person.Key]); // предметы + ученики
                        Dictionary<int, int[]> teachersClassesPerson = new Dictionary<int, int[]>(teachersClasses[person.Key]); // учителя + ученики
                        MutationTeacher(distributionTeachersPerson, distributionClassesPerson, countDifferentMutationType);
                        MutationClass(distributionTeachersPerson, distributionClassesPerson, teachersClassesPerson, countDifferentMutationType);
                        _distributionTeachers.Add(person.Key * countDifferentMutationType * countSameMutationPerson + i * countSameMutationPerson + j, distributionTeachersPerson);
                        _distributionClasses.Add(person.Key * countDifferentMutationType * countSameMutationPerson + i * countSameMutationPerson + j, distributionClassesPerson);
                        _teachersClasses.Add(person.Key * countDifferentMutationType * countSameMutationPerson + i * countSameMutationPerson + j, teachersClassesPerson);
                    }
                }
            }
            this.distributionTeachers = _distributionTeachers;
            this.distributionClasses = _distributionClasses;
            this.teachersClasses = _teachersClasses;
        }


        private void MutationTeacher(Dictionary<int, int[]> distributionTeachers, Dictionary<int, int[]> teachersClasses, int multiplier)
        {
            Random rnd = new Random();
            for (int k = 0; k < 10 + multiplier * 2; k++) // числа 10 и множитель 2 взяты из головы, определение количества мутаций особи
            {
                int mutationTeacher = rnd.Next(0, distributionTeachers.Count);
                VTeacher vMutataionTeacher = teacherStorage.GetElement(new Models.Teacher() { ID = mutationTeacher });
                if (CheckArrayValue(distributionTeachers[mutationTeacher]))
                {
                    var groupProperties = GetGroupInArrayForTeacher(distributionTeachers[mutationTeacher], teachersClasses[mutationTeacher], lowMutationTeacher);
                            
                    VSubject subject = subjectStorage.GetElement(new Models.Subject()
                    {
                        ID = groupProperties.Item1,
                    });
                    List<int> availableTeachers = subject.TeacherSubjects.Keys.ToList(); //выбираем только из учителей, у которых есть такой предмет
                    int swapTeacher = rnd.Next(0, availableTeachers.Count);
                    if (availableTeachers[swapTeacher] == mutationTeacher)
                    {
                        k--;
                        continue;
                    }
                    if (CheckTeacherAvailable(vMutataionTeacher, distributionTeachers[availableTeachers[swapTeacher]], groupProperties.Item2)) // проверяет, есть ли окна у второго преподавателя, есть ли возможность переноса пар
                    {
                        SwapSubjects(distributionTeachers[mutationTeacher], distributionTeachers[availableTeachers[swapTeacher]], groupProperties.Item2 ); // перемешивание внутри текущего времени не влияет на пары учеников
                    }
                    else
                    {
                        k--;
                        continue;
                    }
                }
                else
                {
                    k--;
                }
            }      
        }

        private void MutationClass(Dictionary<int, int[]> distributionTeachers, Dictionary<int, int[]> teachersClasses, Dictionary<int, int[]> distributionClasses, int multiplier) //суть мутации - перемешивание пар внутри расписания класса для внесения
                                                                                                                                                                                  //разнообразия. При помощи перемещения предмета, без его смены
        {
            Random rnd = new Random();
            for (int k = 0; k < 10 + multiplier * 2; k++) // числа 10 и множитель 2 взяты из головы, определение количества мутаций особи
            {
                int mutationClass = rnd.Next(0, distributionClasses.Count);
                        
                if (CheckArrayValue(distributionClasses[mutationClass]))
                {
                    int swapStartIndex = GetRandomNotNullIndex(distributionClasses[mutationClass]);
                    int swapEndIndex = GetRandomNullIndex(distributionClasses[mutationClass]);
                    var subjectID = distributionClasses[mutationClass][swapStartIndex];
                    VSubject subject = subjectStorage.GetElement(new Models.Subject()
                    {
                        ID = subjectID,
                    });
                    int teacherID = 0;
                    for (int indexTeacher = 0; indexTeacher < teachersClasses.Count; indexTeacher++)
                    {
                        if (teachersClasses[indexTeacher][swapStartIndex] == mutationClass && distributionTeachers[indexTeacher][swapStartIndex] == subjectID) //у преподавателя в данное время та же группа и тот же предмет, что даёт право определить,
                                                                                                                                                        //что это верный преподаватель, вторая проверка может быть избыточной
                        {
                            teacherID = indexTeacher;
                        }
                    }
                    // убедиться что на изменяемое время у учителя есть окно или пары можно поменять с другим классом
                    if (!CheckTeacherAvailable(distributionTeachers[teacherID],swapEndIndex) ||
                        (teachersClasses[teacherID][swapEndIndex] != 0 ? !CheckClassAvailable(distributionClasses[teachersClasses[teacherID][swapEndIndex]], swapEndIndex): false))
                    {
                        k--;
                        continue;
                    }
                    else
                    {
                        if (teachersClasses[teacherID][swapEndIndex] != 0)
                        {
                            MoveCouples(distributionClasses[mutationClass], distributionTeachers[teacherID], teachersClasses[teacherID], swapStartIndex, swapEndIndex, subjectID, mutationClass);
                        }
                        else
                        {
                            MoveCouples(distributionClasses[mutationClass], distributionClasses[teachersClasses[teacherID][swapEndIndex]], teachersClasses[teacherID], swapStartIndex, swapEndIndex, subjectID, mutationClass, teachersClasses[teacherID][swapEndIndex]);
                        }
                    }
                }
                else
                {
                    k--;
                }
            }
        }

        private void FitnessFunction()
        {
            foreach (var person in distributionTeachers)
            {
                double teacherEstimate = EstimateTeachers(distributionTeachers[person.Key]);
                double classEstimate = EstimateClasses(distributionClasses[person.Key]);
                personEstimates.Add(person.Key, (teacherEstimate * teacherCoefficient + classEstimate * classCoefficient) / 2);
            }
            Dictionary<int, Dictionary<int, int[]>> _distributionTeachers = new Dictionary<int, Dictionary<int, int[]>>(distributionTeachers);
            Dictionary<int, Dictionary<int, int[]>> _distributionClasses = new Dictionary<int, Dictionary<int, int[]>>(distributionClasses);
            Dictionary<int, Dictionary<int, int[]>> _teachersClasses = new Dictionary<int, Dictionary<int, int[]>>(teachersClasses);
            List<KeyValuePair<int, double>> orderedList = personEstimates.ToList();
            orderedList.Sort((x, y) => x.Value.CompareTo(y.Value));
            for (int i = 0; i < survivingPersons; i++)
            {
                var recordTeacher = distributionTeachers[orderedList[i].Key];
                var recordClass = distributionClasses[orderedList[i].Key];
                var recordTC = teachersClasses[orderedList[i].Key];
                _distributionTeachers.Add(i, recordTeacher);
                _distributionClasses.Add(i, recordClass);
                _teachersClasses.Add(i, recordTC);
            }
            distributionTeachers = _distributionTeachers;
            distributionClasses = _distributionClasses;
            teachersClasses = _teachersClasses;
        }
        private double EstimateTeachers(Dictionary<int, int[]> distributionTeachers)
        {
            double sum = 0;
            foreach (var teacher in distributionTeachers)
            {
                sum += GetEstimateWindow(teacher.Value);
            }
            return sum;
        }

        private double EstimateClasses(Dictionary<int, int[]> distributionClasses)
        {
            double sum = 0;
            foreach (var clazz in distributionClasses)
            {
                sum += GetEstimateCouplesImbalance(clazz.Value);
                sum += GetEstimateWindow(clazz.Value);
                sum += GetEstimateDuplicate(clazz.Value);
                sum += GetEstimatePostponement(clazz.Value);
            }
            return sum;
        }

        private double GetEstimateCouplesImbalance(int[] timetable) 
        {
            double total = 10;
            List<int> countCouples = new List<int>();
            for (int i = 0; i < countWorkDays; i++)
            {
                int startIndex = i * countHoursInDay;
                int endIndex = (i + 1) * countHoursInDay - 1;
                int[] day = timetable[startIndex..endIndex];
                if (!CheckArrayValue(day))
                {
                    total*=0.96; //штраф за выходной день на неделе
                }
                int countCouple = 0;
                for (int j = 0; j < day.Length; j++)
                {
                    if (day[j] != 0)
                    {
                        countCouple++;
                    }
                }
                countCouples.Add(countCouple);
            }
            double average = countCouples.Average();
            foreach (var count in countCouples)
            {
                if (count > average + 2 || count < average - 2)
                {
                    total *= 0.8;
                }
            }
            return total;
        }

        private double GetEstimateWindow(int[] timetable)
        {
            double total = 10;
            int windows = 0;
            for (int i = 0; i < countWorkDays; i++)
            {
                int startIndex = i * countHoursInDay;
                int endIndex = (i + 1) * countHoursInDay - 1;
                int[] day = timetable[startIndex..endIndex];
                bool dayStart = false;
                bool findNull = false;
                for (int j = 0; j < day.Length; j++)
                {
                    if (day[j] != 0)
                    {
                        if (!dayStart)
                        {
                            dayStart = true;
                            continue;
                        }
                        else
                        {
                            if (findNull)
                            {
                                windows++;
                                findNull = false;
                            }
                        }
                    }
                    else 
                    {
                        if (dayStart)
                        {
                            findNull = true;
                        }
                    }
                }
            }
            return total - windows;
        }

        private double GetEstimateDuplicate(int[] timetable)
        {
            double total = 10;
            int duplicate = 0;
            for (int i = 0; i < countWorkDays; i++)
            {
                int startIndex = i * countHoursInDay;
                int endIndex = (i + 1) * countHoursInDay - 1;
                int[] day = timetable[startIndex..endIndex];
                int currentSubject = -1;
                List<int> subjects = new List<int>();
                for (int j = 0; j < day.Length; j++)
                {
                    if (day[j] != 0)
                    {
                        if (day[j] != currentSubject && subjects.Contains(day[j]))
                        {
                            duplicate++;
                        }
                        currentSubject = day[j];
                        subjects.Add(currentSubject);
                        
                    }
                }
            }
            return total - duplicate * 0.4;
        }

        private double GetEstimatePostponement(int[] timetable)
        {
            double total = 10;
            int postponement = 0;
            List<int> lastSubjects = new List<int>();
            List<int> firstSubjects = new List<int>();
            for (int i = 0; i < countWorkDays; i++)
            {
                int startIndex = i * countHoursInDay;
                int endIndex = (i + 1) * countHoursInDay - 1;
                int[] day = timetable[startIndex..endIndex];
                int firstSubject = -1;
                int lastSubject = -1;

                for (int j = 0; j < day.Length; j++)
                {
                    if (day[j] != 0)
                    {
                        lastSubject = day[j];
                        if (firstSubject == -1)
                        {
                            firstSubject=day[j];
                            firstSubjects.Add(day[j]);
                        }
                    }
                }
                lastSubjects.Add(lastSubject);
            }
            for (int i = 0; i < lastSubjects.Count-1; i++)
            {
                if (lastSubjects[i] == firstSubjects[i+1])
                {
                    postponement++;
                }
            }
            return total - postponement * 2.1;
        }

        private bool CheckArrayValue(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] != 0) {
                    return true;
                }
            }
            return false;
        }

        private bool CheckNullValue(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == 0)
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckTeacherAvailable(VTeacher mutationTeacher, int[] distributionForSwap, List<int> indexes) //метод для учителя
        {
            var availableSubjects = mutationTeacher.TeacherSubjects.Keys.ToList();
            foreach (var index in indexes)
            {
                if (distributionForSwap[index] == -1) // -1 Обозначает нерабочее время
                {
                    return false;
                }
                if (distributionForSwap[index] != 0 && !availableSubjects.Contains(distributionForSwap[index])) { //второй учитель в данное время не имеет окна и при этом первый учитель может взять на себя этот предмет
                    return false;
                }
            }
            return true;
        }

        private bool CheckTeacherAvailable(int[] array, int index) //метод для класса
        {
            if (array[index] == -1) // -1 Обозначает нерабочее время
            {
                return false;
            }
            return true;
        }

        private bool CheckClassAvailable(int[] clazz, int startIndex) //метод проверяет могут ли классы поменяться друг с другом уроками (по времени)
        {
            if (clazz[startIndex] != 0) 
            {
                return false;
            }
            return true;
        }

        private void SwapSubjects(int[] firstObject, int[] secondObject, List<int> indexes)
        {
            foreach (var i in indexes)
            {
                var buf = secondObject[i];
                secondObject[i] = firstObject[i];
                firstObject[i] = buf;
            }
        }

        private void MoveCouples(int[] clazz, int[] teacher, int[] teacherClass, int startIndex, int endIndex, int subjectID, int classID) // когда у преподавателя было окно
        {
            teacher[startIndex] = 0;
            teacher[endIndex] = subjectID;
            clazz[startIndex] = 0;
            clazz[endIndex] = subjectID;
            teacherClass[startIndex] = 0;
            teacherClass[endIndex] = classID;
        }

        private void MoveCouples(int[] classFirst, int[] classSecond, int[] teacherClass, int startIndex, int endIndex, int subjectID, int classIDFirst, int classIDSecond) // когда у преподавателя не было окна
        {
            classFirst[startIndex] = 0;
            classFirst[endIndex] = subjectID;
            classSecond[startIndex] = subjectID;
            classSecond[endIndex] = 0;
            teacherClass[startIndex] = classIDSecond;
            teacherClass[endIndex] = classIDFirst;
        }

        private (int, List<int>) GetGroupInArrayForTeacher(int[] subjectTeacher, int[] classTeacher, bool group)
        {
            int randomIndex = GetRandomNotNullIndex(subjectTeacher);
            int subjectID = subjectTeacher[randomIndex];
            int classID = classTeacher[randomIndex];
            List<int> indexes = GetEqualsValueIndexes(subjectTeacher, subjectTeacher[randomIndex]);
            if (!group)
            {
                return  (subjectID, indexes);
            }
            // найти у этого преподавателя уроки одного предмета
            // отфильтровать список на предметы группы (класса) randomIndex
            foreach (var index in indexes)
            {
                if (classTeacher[index] != classID)
                {
                    indexes.Remove(index);
                }
            }

            return new ( subjectID, indexes );
        }

        private List<int> GetEqualsValueIndexes (int[] array, int value)
        {
            List<int> answer = new List<int>();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == value)
                {
                    answer.Add(i);
                }
            }
            return answer;
        }

        private int GetRandomNotNullIndex(int[] array) //обработка нуля не нужна, метод не вызывается, если все значения нулевые
        {
            Random rnd = new Random();
            int randomIndex = rnd.Next(0, array.Length);
            if (array[randomIndex] == 0)
            {
                for (int i = randomIndex; i < array.Length; i++)
                {
                    if (array[i] != 0)
                    {
                        return i;
                    }
                }
                for (int i = 0; i < randomIndex; i++)
                {
                    if (array[i] != 0)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        private int GetRandomNullIndex(int[] array) 
        {
            Random rnd = new Random();
            int randomIndex = rnd.Next(0, array.Length);
            if (array[randomIndex] != 0)
            {
                for (int i = randomIndex; i < array.Length; i++)
                {
                    if (array[i] == 0)
                    {
                        return i;
                    }
                }
                for (int i = 0; i < randomIndex; i++)
                {
                    if (array[i] == 0)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        private Dictionary<int, int[]> InitClasses()
        {
            Dictionary<int, int[]> classes = new Dictionary<int, int[]>();
            List<VClass> classesList = classStorage.GetFullList();
            foreach (var clazz in classesList)
            {
                classes.Add((int)clazz.ID, new int[clazz.Hours]); // рекомендуемое число 6*8 = 48 часов
            }
            return classes;
        }

        private int[] InitTeacher(VTeacher teacher)
        {
            int[] teacherDistribution = new int[countWorkDays*countHoursInDay];
            for (int i=0;i<teacherDistribution.Length;i++)
            {
                teacherDistribution[i] = -1; //заполняем всё время учителя как не рабочее
            }
            foreach (var day in teacher.WorkDays)
            { 
                for (int i = teacher.StartDay; i<=teacher.EndDay;i++)
                {
                    teacherDistribution[day * countHoursInDay + i] = 0;
                }
               
            }
            return teacherDistribution;
        }
    }
}
