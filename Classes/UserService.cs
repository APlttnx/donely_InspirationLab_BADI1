using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace donely_Inspilab.Classes
{
    class UserService
    {
        //Registreren nieuwe user
        public static User Register(string name, string email, string telephoneNumber, string password, string confirmPassword, string profilePicture = "", bool isF2A = false)
        {
            if (!ValidateEmail(email))
                throw new ArgumentException("Invalid email address.");

            if (!PasswordsMatch(password, confirmPassword))
                throw new ArgumentException("Passwords are not the same.");
            string hashedPassword = HashPassword(password);

            return new User(name, email, telephoneNumber, hashedPassword, profilePicture, isF2A);
        }

        private static bool PasswordsMatch(string password, string confirmPassword)
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



        public static User Login(string email, string password)
        {
            Database db = new();
            Credentials creds = db.GetUserCredentialsByEmail(email);
            if (AuthenticatePassword(password, creds.HashedPassword))
            {
                db.UpdateLogin(creds.UserID);
                return db.GetUserByID(creds.UserID);
            }
            else
                throw new ArgumentException("Wrong password or email");
        }

        private static bool AuthenticatePassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }


        public static bool DeleteUser(int id)
        {
            Database db = new();
            if (id != 0)
                return (db.DeleteUser(id) != 0);
            return false;
        }
    }
}
