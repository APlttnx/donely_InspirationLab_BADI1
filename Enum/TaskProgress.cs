using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace donely_Inspilab.Enum
{
    public enum TaskProgress //Oorspronkelijk TaskStatus, maar conflicteerde met System.Threading.Tasks.TaskStatus
    {
        Active,
        Pending,
        Success,
        Failure
    }
}
