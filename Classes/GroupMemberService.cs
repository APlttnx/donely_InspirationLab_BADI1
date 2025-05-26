using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace donely_Inspilab.Classes
{
    public static class GroupMemberService
    {

        public static List<GroupMember> GetListGroupMembers(int groupID)
        {
            //Haalt lijst op van alle groupMembers van opgegeven groupID
            Database db = new();
            return(db.GetGroupMembers(groupID));
        }

    }
}
