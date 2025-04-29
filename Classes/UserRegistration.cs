//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using BCrypt;
//using System.Text.RegularExpressions;
//using System.ComponentModel.DataAnnotations;

//namespace donely_Inspilab.Classes
//{
//    public class UserRegistration
//    {
//        public string Name { get; set; }
//        public string Email { get; set; }
//        public string TelephoneNumber { get; set; }
//        public string Password { get; set; }
//        public string ConfirmPassword { get; set; }
//        private string HashedPassword { get; set; }

//        public UserRegistration(string _name, string _email, string _telephoneNumber, string _password, string _confirmPassword)
//        {
//            Name = _name;
//            Email = _email;
//            TelephoneNumber = _telephoneNumber;
//            Password = _password;
//            ConfirmPassword = _confirmPassword;
//        }
//        private bool CheckPassword()
//        {
//            return this.Password == this.ConfirmPassword;
//        }
//        private void HashPassword()
//        {
//            HashedPassword = BCrypt.Net.BCrypt.HashPassword(Password);
//        }
//        private bool CheckEmail()
//        {
//            string rgx = @"^((?!\.)[\w-_.]*[^.])(@\w+)(\.\w+(\.\w+)?[^.\W])$";
//            return Regex.IsMatch(this.Email, rgx);
//        }

//    }
//}
