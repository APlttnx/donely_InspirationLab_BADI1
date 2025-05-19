using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

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
            ProfilePicture = _profilePicture;
            Id = _id;
            AccountCreated = _accountCreated;
            LastLogin = _lastLogin;
            IsAdmin = _isAdmin;
        }
        public User(string _name, string _email, string _telephoneNumber, string _hashedPassword, string _profilePicture = "Default", bool is2FA = false) //partial constructor register/new user
        {
            Name = _name;
            Email = _email;
            TelephoneNumber = _telephoneNumber;
            HashedPassword = _hashedPassword;
            ProfilePicture = _profilePicture;
            Is2FA = is2FA;
        }   
    }
}
