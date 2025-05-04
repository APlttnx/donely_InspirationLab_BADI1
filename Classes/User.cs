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
        public string HashedPassword { get; private set; } //zie later -> spec voor paswoord aan db toe te voegen, te zien of later rond te werken.
        public int? Id { get; private set; } //? = nullable
        public DateTime? AccountCreated { get; private set; } 
        public DateTime? LastLogin { get; private set; } 

        private User(string _name, string _email, string _telephoneNumber, string _hashedPassword/*, profilePicture*/) //partial constructor register/new user
        {
            Name = _name;
            Email = _email;
            TelephoneNumber = _telephoneNumber;
            HashedPassword = _hashedPassword;
        }

        public User(string _name, string _email, string _telephoneNumber, string _hashedPassword,  int _id, DateTime _accountCreated, DateTime _lastLogin) : this(_name, _email, _telephoneNumber, _hashedPassword) //full constructor
        {
            Id = _id;
            AccountCreated = _accountCreated;
            LastLogin = _lastLogin;
        }

        public static User Register(string name, string email, string telephoneNumber, string password, string confirmPassword)
        {
            if (!ValidateEmail(email))
                throw new ArgumentException("Invalid email address.");

            if (!PasswordsMatch(password, confirmPassword))
                throw new ArgumentException("Passwords are not the same.");
            string hashedPassword = HashPassword(password);
            
            return new User(name, email, telephoneNumber, hashedPassword);
        }
        private static bool PasswordsMatch( string password, string confirmPassword)
        {
            return password == confirmPassword;
        }
        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        private static bool ValidateEmail(string email)
        {
            string rgx = @"^((?!\.)[\w-_.]*[^.])(@\w+)(\.\w+(\.\w+)?[^.\W])$";
            return Regex.IsMatch(email, rgx);
        }

        
    }
}
