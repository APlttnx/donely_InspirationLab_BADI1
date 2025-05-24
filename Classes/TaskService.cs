using donely_Inspilab.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace donely_Inspilab.Classes
{
    public static class TaskService
    {
        public static Task CreateTask(string name, string description, int reward, TaskFrequency frequency, bool validationRequired, int groupId)
        {
            //Validations 
            if (reward > 0 ) throw new ArgumentException("Reward cannot be a negative number.");
            

            Task newTask = new(name, description, reward, frequency, validationRequired, groupId, true);
            Database db = new();
            newTask.Id = db.InsertTaskDefinition(newTask);
            return newTask;
        }

        public static TaskInstance CreateTaskInstance(Task task, DateTime deadline, GroupMember member)
        {
            if (task.Frequency == TaskFrequency.None && deadline == DateTime.MinValue) throw new ArgumentException("Deadline wasn't specified for a one-day exercise");
            if (deadline < DateTime.Now) throw new ArgumentException("Deadline can't be before today");
            TaskInstance newInstance = new(task, member.UserId, deadline);
            Database db = new();
            newInstance.Id = db.InsertTaskInstance(newInstance);
            return newInstance;
        }
        //TODO
        //public static TaskInstance CreateNextRecurringInstance(TaskInstance previousTask, GroupMember member, DateTime lastDeadline)
        //{
        //    previousTask.Task.
        //    if (task.Frequency == TaskFrequency.None)
        //        throw new InvalidOperationException("Non-recurring task can't be scheduled again.");

        //    var nextDeadline = CreateNextDeadline(task, lastDeadline);
        //    return TaskInstance.CreateTaskInstance(task, nextDeadline, member);
        //}
        //private static DateTime CreateNextDeadline(Task task, DateTime previousDeadline)
        //{
        //    DateTime deadline = new DateTime();
        //    //DateTime endOfDay = DateTime.Now.Date.AddDays(1).AddMilliseconds(-1);
        //    switch (task.Frequency)
        //    {
        //        case TaskFrequency.None:
        //            throw new ArgumentException("This task is not a recurring task");
        //        case TaskFrequency.Daily:
        //            deadline = previousDeadline.AddDays(1);
        //            break;
        //        case TaskFrequency.Weekly:
        //            deadline = previousDeadline.AddDays(7);
        //            break;
        //        case TaskFrequency.Monthly:
        //            deadline = previousDeadline.AddMonths(1);
        //            break;
        //        default: return DateTime.MinValue;
        //    }
        //     return deadline;
            
        }
    }
}
