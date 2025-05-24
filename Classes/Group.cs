using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace donely_Inspilab.Classes
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageLink { get; set; }
        public User Owner { get; set; }
        public DateTime CreationDate { get; set; }
        public string InviteCode { get; set; }

        //Lists worden manueel gevuld via aparte methodes in de service wanneer ze nodig zijn (zoals op Group Owner dashboard of Member dashboard)
        public List<ShopItem> ShopItems { get; set; } = new();
        public bool ShopActive => ShopItems.Count > 0;

        public List<GroupMember> Members { get; set; } = new(); 
        //public List<TaskDefinition> TaskDefinitions { get; set; } = new();
        //public List<TaskInstance> ActiveTasks { get; set; } = new();

        //Create new group Constructor
        public Group(string _name, User _owner, string _imageLink = "groupImages/default.png")
        {
            Name = _name;
            Owner = _owner ?? throw new ArgumentNullException(nameof(_owner));  
            ImageLink = _imageLink;
        }

        // Full constructor
        public Group(int _id, string _name, User _owner, DateTime _creationDate, string _imageLink, string _inviteCode)
        {
            Id = _id;
            Name = _name;
            Owner = _owner ?? throw new ArgumentNullException(nameof(_owner));
            CreationDate = _creationDate;
            ImageLink = _imageLink;
            InviteCode = _inviteCode;
        }


    }
}
