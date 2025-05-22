using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace donely_Inspilab.Classes
{
    public static class GroupState
    {
        // GroupState houdt de info bij van geladen groepen, en roept gedetailleerde info op zoals member lists en task lists op.
        // Zo kunnen verschillende pagina's zoals TaskPage en GroupDashboardPage hier gebruik van maken zonder desbetreffende groep telkens mee te geven via NavService.
        // Deze wordt gereset bij het herladen van de homepage en bij logout
        public static Group? LoadedGroup { get; set; }

        public static bool IsGroupLoaded => LoadedGroup != null;

        public static void ClearGroup() => LoadedGroup = null;

        public static void LoadGroup(Group group)
        {
            if (LoadedGroup != null && group.Id == LoadedGroup.Id) return; //als geselecteerde groep al geladen is. Zal normaal gezien nooit triggeren aangezien ClearGroup call bij navigeren HomePage
            group.Members = GroupMemberService.GetListGroupMembers(group.Id);
            //TODO: Tasks en shopitems ook ophalen
            LoadedGroup = group;
        }


    }
}
