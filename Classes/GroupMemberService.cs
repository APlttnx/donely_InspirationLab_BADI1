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
        public static void KickMember(GroupMember member)
        {
            Database db = new();
            db.DeleteGroupMember(member.Id);
        }

        public static int AddCurrency(GroupMember member, int currency)
        {
            member.Currency += currency;
            Database db = new();
            db.UpdateMemberCurrency(member.Id, member.Currency);

            return member.Currency;
        }

    }
}
