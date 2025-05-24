using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using donely_Inspilab.Enum;

namespace donely_Inspilab.Classes
{
    public static class TaskService
    {
        public static Task CreateTask(string name, string description, int reward, TaskFrequency frequency,  DateOnly deadline, bool validationRequired, int groupId)
        {
            //Validations 
            if (reward > 0 ) throw new ArgumentException("Reward cannot be a negative number.");
            if (frequency == 0 && deadline == null) throw new ArgumentException("Deadline wasn't specified for a one-day exercise");
            if (deadline < DateOnly.FromDateTime(DateTime.Now)) throw new ArgumentException("Deadline can't be before today");

            Task newTask = new(name, description, reward, deadline, frequency, validationRequired, groupId, true);
            Database db = new();
            newTask.Id = db.InsertTaskDefinition(newTask);
            return newTask;

        }
    }
}
