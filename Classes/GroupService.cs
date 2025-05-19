using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using donely_Inspilab.Classes;
using donely_Inspilab.Pages;

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
            newGroup.Id = db.InsertGroup(newGroup);
            return newGroup;
        }


    }
}
