using donely_Inspilab.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace donely_Inspilab.Classes
{
    public static class TaskService
    {
        //CREATE TASK
        public static Task CreateTask(string name, string description, int reward, TaskFrequency frequency, bool validationRequired, int groupId)
        {
            //Validations 
            if (reward < 0 ) throw new ArgumentException("Reward cannot be a negative number.");
            

            Task newTask = new(name, description, reward, frequency, validationRequired, groupId, true);
            Database db = new();
            int id = db.InsertTaskDefinition(newTask);
            return db.GetTaskById(id); //onmiddelijk geüpdatete taak uit database halen --> zeker hetzelfde
        }
        //READ TASK
        public static List<Task> GetGroupDefinitions(int groupId)
        {
            Database db = new();
            return (db.GetGroupTaskDefinitions(groupId));
        }

        //UPDATE TASK
        public static Task UpdateTask(int id, string name, string description, int reward, TaskFrequency frequency, bool validationRequired)
        {
            if (reward < 0)
                throw new ArgumentException("Reward cannot be a negative number.");

            Database db = new();
            int rowsAffected = db.UpdateTaskDefinition(new Task(id, name, description, reward, frequency, validationRequired, true, 0));

            if (rowsAffected != 1)
                throw new Exception("Failed to update task in database.");

            return db.GetTaskById(id);
        }
        
        //UPDATE TASK - Toggle Active
        public static void ToggleTaskIsActive(int taskId, bool isActive)
        {
            Database db = new();
            db.UpdateTaskIsActive(taskId, isActive);
        }

        //DELETE (Actually update) TASK
        public static void SoftDeleteTask(int taskId)
        {
            Database db = new();
            db.SoftDeleteTask(taskId);
        }

        //CREATE INSTANCE
        public static TaskInstance CreateTaskInstance(Task task, DateTime deadline, int memberId)
        {
            if (task == null) throw new ArgumentException("No task selected");
            if (deadline.Date < DateTime.Now.Date) throw new ArgumentException("Deadline must be in the future");
            TaskInstance newInstance = new(task, memberId, deadline);
            Database db = new();
            newInstance.Id = db.InsertTaskInstance(newInstance);
            return newInstance;
        }

        //READ INSTANCES FROM MEMBER
        public static GroupMember LoadAndAssignTaskInstances(GroupMember member)
        {
            Database db = new();
            member.LoadTaskList(db.GetTaskInstancesMemberId(member.Id));
            return member;
        }

        //UPDATE Instance
        public static void UpdateTaskInstance(TaskInstance task)
        {
            Database db = new();
            db.UpdateTaskInstance(task);
        }

        //BATCH AUTOFAILER Global --> Runt bij login om alle expired tasks te failen via een query
        public static int AutoFailExpiredTasksGlobal()
        {
            Database db = new();
            return db.AutoFailExpiredTasksGlobal();
        }
        // AUTO REASSIGNER --> Deze methode roept eerst alle Instances op die moeten herhaald worden, checkt de frequency van de task definition en maakt automatisch een nieuwe Instance met nieuwe deadline aan
        // Dailies --> huidige dag zelf
        // Weekly --> een week na de vorige deadline
        // Monthly --> een maand na de vorige deadline
        // roept dat CreateInstance op (Potentieel ToDo voor later --> Batch Inserter query aanmaken)
        public static void AutoReassignRecurringTasks()
        {
            Database db = new();
            List<TaskInstance> tasksToReassign = db.GetAllCompletedRecurringTasks();

            foreach (TaskInstance instance in tasksToReassign)
            {
                DateTime prevDeadline = instance.Deadline;
                DateTime newDeadline;
                DateTime now = DateTime.Now;
                switch (instance.Task.Frequency)
                {
                    case TaskFrequency.Daily:
                        newDeadline = now.Date; //als geen actieve dailies --> nieuwe actieve deadline is vandaag
                        break;
                    case TaskFrequency.Weekly:
                        newDeadline = now > prevDeadline ? prevDeadline.AddDays(7) : DateTime.MinValue;
                        break;
                    case TaskFrequency.Monthly:
                        newDeadline = now > prevDeadline ? prevDeadline.AddMonths(1) : DateTime.MinValue;
                        break;
                    default:
                        newDeadline = new();
                        break;
                }
                if (newDeadline != DateTime.MinValue)
                    CreateTaskInstance(instance.Task, newDeadline, instance.MemberId);
            }
        }

        //GET ALL TASKS OF USER ACROSS GROUPS
        public static List<OngoingTasksView> GetOngoingTasksOfUser(int userID)
        {
            Database db = new();
            return(db.GetAllTasksOfUser(userID));
        }
        
    }
}
