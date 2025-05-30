using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using donely_Inspilab.Exceptions;

namespace donely_Inspilab.Classes
{
    class UserService
    {
        //CREATE USER + Register regels
        public static User Register(string name, string email, string telephoneNumber, string password, string confirmPassword, string profilePicture = "", bool isF2A = false)
        {
            try
            {
                //zeker zijn dat er geen spaties inzitten, en caps to lower om zeker te zijn dat Duplicate Email check zeker werkt
                name = name?.Trim();
                email = email?.Trim().ToLowerInvariant();
                telephoneNumber = telephoneNumber?.Trim();


                if (!ValidateEmail(email))
                    throw new ArgumentException("Invalid email address.");

                if (!PasswordsMatch(password, confirmPassword))
                    throw new ArgumentException("Passwords are not the same.");
                string hashedPassword = HashPassword(password);

                User user = new(name, email, telephoneNumber, hashedPassword, profilePicture, isF2A);

                try
                {
                    Database db = new();
                    int affected = db.InsertUser(user);
                    if (affected != 1)
                        throw new DataAccessException("Failed to insert user.");
                }
                catch (DuplicateException)
                {
                    throw new ArgumentException("This email is already in use.");
                }

                return user;

            }
            catch (DuplicateEmailException)
            {
                throw new ArgumentException("This email is already in use.");
            }


        }

        //Interne methode - Check of wachtwoorden wel overeenkomen
        private static bool PasswordsMatch(string password, string confirmPassword)
        {
            return password == confirmPassword;
        }
        //Interne methode - hashing password via BCrypt
        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        //Interne methode - Valideren via .NET methode of email wel valide is
        private static bool ValidateEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }


        //READ user --> via Credentials klasse controleren of ingevuld emailadres en wachtwoord overeenkomen voor een vd users
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

        //Interne methode - check via BCrypt verify of wachtwoord juist is
        private static bool AuthenticatePassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        // DELETE USER
        public static bool DeleteUser(int id)
        {
            Database db = new();
            if (id != 0)
                return (db.DeleteUser(id) != 0);
            return false;
        }

        // UPDATE USER
        public static void UpdateUser(User user)
        {
            Database db = new();
            db.UpdateUser(user);
        }
    }
}
