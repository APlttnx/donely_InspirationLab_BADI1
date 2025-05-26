using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using donely_Inspilab.Enum;
using donely_Inspilab.Methods;

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
        public DateTime? CompletionDate { get; set; }
        
        //Constructor Creating new instance
        public TaskInstance(Task _task, int _memberId, DateTime _deadline)
        {
            Task = _task;
            TaskId = _task.Id;
            MemberId = _memberId;
            Deadline = _deadline;
            Status = TaskProgress.Active; // bij creatie is TaskProgress altijd actief. 
            // IssueDate is set by DB, do not assign it here
        }
        //Full Constructor
        public TaskInstance(int _id, Task _task, int _memberId, DateTime _deadline, TaskProgress _status, DateTime _issueDate, DateTime? _completionDate)
            : this(_task, _memberId, _deadline)
        {
            Id = _id;
            Status = _status; //Override original status if necessary
            IssueDate = _issueDate;
            CompletionDate = _completionDate;
        }
        public DateOnly DeadlineDateOnly => DateOnly.FromDateTime(Deadline);

        //Voor gebruik bij Listviews
        public string DeadlineDisplay => DateFormatter.FormatDate(Deadline);
        public string IssueDateDisplay => DateFormatter.FormatDate(IssueDate);
        public string CompletionDateDisplay => DateFormatter.FormatDate((DateTime)CompletionDate);
    }
}
