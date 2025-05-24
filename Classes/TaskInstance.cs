using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using donely_Inspilab.Enum;

namespace donely_Inspilab.Classes
{
    public class TaskInstance
    {
        public int Id { get; set; }
        public Task Task { get; set; }
        public int TaskId {  get; set; }
        public int MemberId { get; set; }
        public DateTime Deadline { get; set; }
        public TaskProgress Status { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime CompletionDate { get; set; }

        //Constructor Creating new instance
        public TaskInstance(Task _task, int _memberId, DateTime _deadline)
        {
            Task = _task;
            TaskId = _task.Id;
            MemberId = _memberId;
            Deadline = _deadline;
            Status = TaskProgress.Active;
            // IssueDate is set by DB, do not assign it here
        }

        //Full Constructor Creating new instance

    }
}
