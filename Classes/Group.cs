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
        public List<ShopItem> ShopItems { get; set; } = new();
        public bool ShopActive => ShopItems.Count > 0;

        //public List<User> Members { get; set; } = new(); // via GroupUsers join table
        //public List<TaskDefinition> TaskDefinitions { get; set; } = new();
        //public List<TaskInstance> ActiveTasks { get; set; } = new();

        public ImageSource ImageSource
        {
            get
            {
                string? fullPath = !string.IsNullOrWhiteSpace(ImageLink) 
                    ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "GroupImages", ImageLink) 
                    : null;

                if (string.IsNullOrEmpty(fullPath) || !File.Exists(fullPath))
                {
                    // Return a default embedded resource or local image to indicate "no image"
                    // For example, "Assets/default_profile.png"
                    string defaultPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "default.png");
                    if (File.Exists(defaultPath))
                        return new BitmapImage(new Uri(defaultPath, UriKind.Absolute));

                    return null;
                }

                var image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(fullPath, UriKind.Absolute);
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();

                return image;
            }
        }


        //Create new group Constructor
        public Group(string _name, User _owner, string _imageLink = "groupImages/default.png")
        {
            Name = _name;
            Owner = _owner ?? throw new ArgumentNullException(nameof(_owner));  
            ImageLink = _imageLink;
        }

        // Full constructor (lists are separate methods)
        public Group(int _id, string _name, User _owner, DateTime _creationDate, string _imageLink)
        {
            Id = _id;
            Name = _name;
            Owner = _owner ?? throw new ArgumentNullException(nameof(_owner));
            CreationDate = _creationDate;
            ImageLink = _imageLink;
        }


    }
}
