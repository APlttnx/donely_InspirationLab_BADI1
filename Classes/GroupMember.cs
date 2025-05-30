using donely_Inspilab.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace donely_Inspilab.Classes
{
    //Association class --> Combo van User en Group
    // Elke member heeft een een Currency, dat zijn "punten" bijhoudt, net zoals de taskInstances die actief zijn voor die user
    public class GroupMember
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public int GroupId { get; set; }

        public int Currency { get; set; }

        public string Role { get; set; }

        public List<ShopItem> BoughtItems { get; set; } = new();
        public DateTime Joined { get; set; }

        //Counters om te zien hoeveel taken elk lid heeft
        public int ActiveTaskCount { get; set; }
        public int PendingTaskCount { get; set; }
        public int CompletedTaskCount { get;set; }

        //Lijsten voor specifiek aantak taken, worden ingeladen via aparte methodes
        public List<TaskInstance> AllTaskInstances = new();

        public List<TaskInstance> ActiveTaskList { get; private set; } = new();
        public List<TaskInstance> PendingTaskList { get; private set; } = new();
        public List<TaskInstance> CompletedTaskList { get; private set; } = new();


        //Full constructor
        public GroupMember(int _id, User _user, int _groupID, int _currency, List<ShopItem> _boughtItems, DateTime _joined, int _activeTaskCount, int _pendingTaskCount, int _completedTaskCount)
        {
            Id = _id;
            UserId = (int)_user.Id;
            User = _user;
            GroupId = _groupID;
            Currency = _currency;
            BoughtItems = _boughtItems;
            Joined = _joined;
            ActiveTaskCount = _activeTaskCount;
            PendingTaskCount = _pendingTaskCount;
            CompletedTaskCount = _completedTaskCount;            
        }
        //Partial constructor voor nieuwe member aan groep toe te voegen
        public GroupMember(int _groupID, int _userID, int _initialCurrency = 0, string _role = "member") 
        {
            UserId = _userID;
            GroupId = _groupID;
            Currency = _initialCurrency;
            Role = _role;
        }

        //Takenlijst wordt als parameter doorgegeven en toegewezen aan AllTaskinstances (Als applicatie met meer gebruikers en lang gebruikt --> Best aanpassen want potentieel zware query)
        public void LoadTaskList(List<TaskInstance> tasks)
        {
            AllTaskInstances = tasks;
            AutoFailExpiredTasks(); //Autofailed alle expired tasks mocht deadline overschreven zijn
            RefreshFilteredLists(); 
        }

        //Filtert Alle instances in 3 categoriën --> Actieve taken, Pending taken voor goedkeuring owner, en Taken uit het verleden (zowel success als failed). Updatet ook de counters
        public void RefreshFilteredLists()
        {
            ActiveTaskList = AllTaskInstances
                .Where(t => t.Status == TaskProgress.Active)
                .OrderBy(t => t.Deadline)
                .ToList();

            PendingTaskList = AllTaskInstances
                .Where(t => t.Status == TaskProgress.Pending)
                .OrderBy(t => t.CompletionDate)
                .ToList();

            CompletedTaskList = AllTaskInstances
                .Where(t => t.Status == TaskProgress.Success || t.Status == TaskProgress.Failure)
                .OrderByDescending(t => t.CompletionDate) //meest recente completions eerst
                .ToList();

            ActiveTaskCount = ActiveTaskList.Count;
            PendingTaskCount = PendingTaskList.Count;
            CompletedTaskCount = CompletedTaskList.Count;
        }

        public void AutoFailExpiredTasks() //Autofail alle taken in de lijst waar deadline overschreden is
        {
            DateOnly now = DateOnly.FromDateTime(DateTime.Now);
            var ExpiredTasks = AllTaskInstances.Where(t => t.Status == TaskProgress.Active && t.DeadlineDateOnly < now); 
            foreach (TaskInstance task in ExpiredTasks)
            {
                task.Status = TaskProgress.Failure;
                task.CompletionDate = DateTime.Now;
                TaskService.UpdateTaskInstance(task);
            }
            RefreshFilteredLists();
        }

        public void UpdateTaskStatus(TaskInstance updatedTask) //Als bepaalde taak van status veranderd --> ook in actieve klasse aanpassen
        {
            int index = -1;
            index = AllTaskInstances.FindIndex(t => t.Id == updatedTask.Id);
            if (index != -1)
            {
                AllTaskInstances[index] = updatedTask;
                RefreshFilteredLists();
            }
        }
    }
}
