using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace donely_Inspilab.Classes
{
    //Association class
    public class GroupMember
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public int GroupId { get; set; }

        public int Currency { get; set; }

        public string Role { get; set; }

        public List<ShopItem> BoughtItems { get; set; } = new();
        public DateTime Joined { get; set; }

        public GroupMember(int _id, User _user, int _groupID, int _currency, List<ShopItem> _boughtItems, DateTime _joined)
        {
            Id = _id;
            UserId = (int)_user.Id;
            User = _user;
            GroupId = _groupID;
            Currency = _currency;
            BoughtItems = _boughtItems;
            Joined = _joined;
        }
        public GroupMember(int _groupID, int _userID, int _initialCurrency = 0, string _role = "member") //Initial constructor voor nieuwe member aan groep toe te voegen
        {
            UserId = _userID;
            GroupId = _groupID;
            Currency = _initialCurrency;
            Role = _role;
        }

    }
}
