using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace donely_Inspilab.Classes
{
    public class UserRegistration
    {
        [Required] public string UserName { get; set; }
        [Required] public string Email { get; set; }
        [Required] public string TelephoneNumber { get; set; }
        [Required] public string Password { get; set; }
        [Required] public string ConfirmPassword { get; set; }
        private string HashedPassword { get; set; }

        public UserRegistration(string _userName, string _email, string _telephoneNumber, string _password, string _confirmPassword)
        {
            UserName = _userName;
            Email = _email;
            TelephoneNumber = _telephoneNumber;
            Password = _password;
            ConfirmPassword = _confirmPassword;
        }
        private bool CheckPassword()
        {
            return this.Password == this.ConfirmPassword;
        }
        private void HashPassword()
        {
            HashedPassword = BCrypt.Net.BCrypt.HashPassword(Password);
        }
        private bool CheckEmail()
        {
            string rgx = @"^((?!\.)[\w-_.]*[^.])(@\w+)(\.\w+(\.\w+)?[^.\W])$";
            return Regex.IsMatch(this.Email, rgx);
        }
        //public object ValidateFields()
        //{
        //    if (!CheckEmail())
        //    {
        //        return ("Email is invalid, please fill in a valid email address!",);
        //    }
        //    if (!CheckPassword())
        //    {
        //        return "Passwords are not the same!";
        //    }
                
        //}

    }
}
