using donely_Inspilab.Classes;
using donely_Inspilab.Exceptions;
using donely_Inspilab.Methods;
using donely_Inspilab.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace donely_Inspilab.Classes
{
    class GroupService
    {
        //CREATE GROUP
        public static Group CreateGroup(string name, string imageLink, List<ShopItem> shopItems)
        {
            Database db = new();
            if (SessionManager.IsLoggedIn == false) 
                throw new InvalidOperationException("User must be logged in to create a group.");
            if (string.IsNullOrWhiteSpace(name)) 
                throw new ArgumentException("Group name is required.");

            Group newGroup = new Group(name, SessionManager.CurrentUser, imageLink);
            
            if (shopItems != null) 
                newGroup.ShopItems = shopItems;
            int i = 0;
            do
            {
                newGroup.InviteCode = CodeGenerator.Generate();
                i++;
            }while (!db.CheckInviteCode(newGroup.InviteCode) && i < 100 /*Exit fallback*/);;
            if (i == 100)
                throw new ArgumentException("Failed to create unique invite code");
            newGroup.Id = db.InsertGroup(newGroup);
            return newGroup;
        }

        //CREATE GROUPMEMBER via Invite Code
        public static int JoinGroupViaCode(string code, int userID)
        {
            Database db = new();
            //via code een aantal groepseigenschappen ophalen
            var (groupID, groupName, ownerID) = db.GetGroupIdByInviteCode(code);
            if (db.MemberPresentInGroup(groupID, userID))
                throw new DuplicateException("You are already part of this group!");
            var result = MessageBox.Show($"Are you sure you want to join group {groupName}", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No) 
                return -1;
            string role = ownerID == userID ? "owner" : "member"; //rol expliciet zetten zodat de owner niet per ongeluk een member wordt
            GroupMember newMember = new GroupMember(groupID, userID, 0, role);
            int groupMemberID = db.InsertNewGroupMember(newMember);
            return groupMemberID;
        }

        //READ GROUP (member of)
        public static List<Group> GetOverviewGroups(int userID)
        {
            Database db = new();
            return(db.GetGroupOverview(userID));

        }
        //READ GROUP (owner of)
        public static List<Group> GetOverviewOwnGroups(User user)
        {
            Database db = new();
            return(db.GetOwnGroups(user));
        }
        //UPDATE GROUP
        public static void UpdateGroup(Group group)
        {
            if (group == null) throw new ArgumentNullException(nameof(group));
            if (!SessionManager.IsLoggedIn) throw new InvalidOperationException("User must be logged in to edit a group.");

            Database db = new();
            db.UpdateGroup(group);
        }
        //DELETE GROUP
        public static void DeleteGroup(int groupID)
        {
            Database db = new();
            db.DeleteGroup(groupID);
        }


    }
}
