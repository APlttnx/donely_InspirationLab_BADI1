using donely_Inspilab.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace donely_Inspilab.Classes
{
    //Association class
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

        public int ActiveTaskCount { get; set; }
        public int PendingTaskCount { get; set; }
        public int CompletedTaskCount { get;set; }


        private List<TaskInstance> AllTaskInstances = new();

        public List<TaskInstance> ActiveTaskList { get; private set; } = new();
        public List<TaskInstance> PendingTaskList { get; private set; } = new();
        public List<TaskInstance> CompletedTaskList { get; private set; } = new();


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
        public GroupMember(int _groupID, int _userID, int _initialCurrency = 0, string _role = "member") //Initial constructor voor nieuwe member aan groep toe te voegen
        {
            UserId = _userID;
            GroupId = _groupID;
            Currency = _initialCurrency;
            Role = _role;
        }

        public void LoadTaskList(List<TaskInstance> tasks)
        {
            AllTaskInstances = tasks;
            AutoFailExpiredTasks();
            RefreshFilteredLists();
        }

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
        }

        public void AutoFailExpiredTasks()
        {
            DateOnly now = DateOnly.FromDateTime(DateTime.Now);
            var ExpiredTasks = AllTaskInstances.Where(t => t.Status == TaskProgress.Active && t.DeadlineDateOnly < now); 
            foreach (TaskInstance task in ExpiredTasks)
            {
                task.Status = TaskProgress.Failure;
                task.CompletionDate = DateTime.Now;
                TaskService.UpdateTask(task);
            }
            RefreshFilteredLists();
        }

    }
}
