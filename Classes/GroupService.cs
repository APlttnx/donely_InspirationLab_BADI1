using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using donely_Inspilab.Classes;
using donely_Inspilab.Pages;
using donely_Inspilab.Methods;
using System.Windows;

namespace donely_Inspilab.Classes
{
    class GroupService
    {
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
            }while (!db.CheckInviteCode(newGroup.InviteCode) && i < 25 /*Exit fallback*/);;
            if (i == 100)
                throw new ArgumentException("Failed to create unique invite code");
            newGroup.Id = db.InsertGroup(newGroup);
            
            return newGroup;
        }

        public static int JoinGroupViaCode(string code, int userID)
        {
            Database db = new();
            var (groupID, groupName) = db.GetGroupIdByInviteCode(code);
            var result = MessageBox.Show($"Are you sure you want to join group {groupName}", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No) return -1;
            int groupMemberID = db.InsertNewGroupMember(groupID, userID, 0, "member");
            return groupMemberID;
        }


    }
}
