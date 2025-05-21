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
        public int UserId { get; set; }
        public User User { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }

        public int Currency { get; set; }

        public List<ShopItem> BoughtItems { get; set; } = new();
        public DateTime Joined { get; set; }

        public GroupMember(User _user, Group _group, int _currency, List<ShopItem> _boughtItems, DateTime _joined)
        {
            UserId = (int)_user.Id;
            User = _user;
            GroupId = _group.Id;
            Group = _group;
            Currency = _currency;
            BoughtItems = _boughtItems;
            Joined = _joined;
        }
        public GroupMember(User user, Group group, int initialCurrency = 0) //Initial constructor voor nieuwe member aan groep toe te voegen
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
            Group = group ?? throw new ArgumentNullException(nameof(group));
            UserId = (int)user.Id;
            GroupId = group.Id;
            Currency = initialCurrency;
        }

    }
}
