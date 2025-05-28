using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace donely_Inspilab.Classes
{
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string TelephoneNumber { get; set; }
        public string? HashedPassword { get; private set; } // Gebruikt bij Registratie/aanmaken
        public bool? Is2FA { get; private set; }  // enkel gebruikt bij Login/Registratie
        public string ProfilePicture { get; set; }
        public int? Id { get; private set; } 
        public DateTime? AccountCreated { get; private set; } 
        public DateTime? LastLogin { get; private set; }
        public bool IsAdmin { get; private set; } = false;

        public User(string _name, string _email, string _telephoneNumber, string _profilePicture, int _id, DateTime _accountCreated, DateTime _lastLogin, bool _isAdmin)//full constructor min wachtwoord
        {
            Name = _name;
            Email = _email;
            TelephoneNumber = _telephoneNumber;
            ProfilePicture = _profilePicture ?? "profilePicture.jpg";
            Id = _id;
            AccountCreated = _accountCreated;
            LastLogin = _lastLogin;
            IsAdmin = _isAdmin;
        }
        public User(string _name, string _email, string _telephoneNumber, string _hashedPassword, string _profilePicture = "profilePicture.jpg", bool is2FA = false) //partial constructor register/new user
        {
            Name = _name;
            Email = _email;
            TelephoneNumber = _telephoneNumber;
            HashedPassword = _hashedPassword;
            ProfilePicture = _profilePicture;
            Is2FA = is2FA;
        }

        public ImageSource ImageSource
        {
            get
            {
                string? fullPath = !string.IsNullOrWhiteSpace(ProfilePicture)
                    ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "ProfilePictures", ProfilePicture)
                    : null;

                if (string.IsNullOrEmpty(fullPath) || !File.Exists(fullPath))
                {
                    // Return a default embedded resource or local image to indicate "no image"
                    // For example, "Assets/default_profile.png"
                    string defaultPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets","ProfilePictures" ,"profilePicture.jpg");
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
    }
}
