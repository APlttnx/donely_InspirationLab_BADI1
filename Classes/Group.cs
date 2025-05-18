using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace donely_Inspilab.Classes
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageLink { get; set; }
        public User Owner { get; set; }
        public DateTime CreationDate { get; set; }
        public bool ShopActive { get; set; }


        public List<ShopItem> ShopItems { get; set; } = new();
        //public List<User> Members { get; set; } = new(); // via GroupUsers join table
        //public List<TaskDefinition> TaskDefinitions { get; set; } = new();
        //public List<TaskInstance> ActiveTasks { get; set; } = new();

        //Create new group Constructor
        public Group(string _name, User _owner, string _imageLink = "default.jpg", bool _shopActive = false)
        {
            Name = _name;
            Owner = _owner ?? throw new ArgumentNullException(nameof(_owner));  
            ImageLink = _imageLink;
            ShopActive = _shopActive;

        }

        // Full constructor (lists are seperate methods)
        public Group(int _id, string _name, User _owner, DateTime _creationDate, string _imageLink, bool _shopActive)
        {
            Id = _id;
            Name = _name;
            Owner = _owner ?? throw new ArgumentNullException(nameof(_owner));
            CreationDate = _creationDate;
            ImageLink = _imageLink;
            ShopActive = _shopActive;
        }


    }
}
