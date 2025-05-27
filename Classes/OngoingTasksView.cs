using donely_Inspilab.Enum;
using donely_Inspilab.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace donely_Inspilab.Classes
{
    public class OngoingTasksView //specifiek voor homepage listview te vullen met huidig actieve taken van de ingelogde user
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }

        public string TaskName { get; set; }
        public int Reward { get; set; }

        public DateTime Deadline { get; set; }
        public TaskFrequency Frequency { get; set; }

        public string DeadlineDisplay => DateFormatter.FormatDate(Deadline);
        
        public string TaskNameDisplay => Frequency != TaskFrequency.Once ? $"[{Frequency.ToString().ToUpper()}] {TaskName}" : TaskName;

        public string TimeLeft => CalcTimeLeft();
        private string CalcTimeLeft()
        {
            DateTime corrDeadline = Deadline.AddDays(1).AddMinutes(-1); //deadline default op 00.00, dus zeker maken dat de waarde EOD is
            TimeSpan time = corrDeadline - DateTime.Now;
            return $"{time.Days} Days {time.Hours} Hours";
        }



        public OngoingTasksView(int _groupId, string _groupName, string _taskName, int _reward, DateTime _deadline, TaskFrequency frequency )
        {
            GroupId = _groupId;
            GroupName = _groupName;
            TaskName = _taskName;
            Reward = _reward;
            Deadline = _deadline;
            Frequency = frequency;
        }
    }
}
