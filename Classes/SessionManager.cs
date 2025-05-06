using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace donely_Inspilab.Classes
{
    public static class SessionManager
    {
        public static User CurrentUser { get; private set; }
        public static bool IsLoggedIn => CurrentUser != null;
        public static void Login(User _user)
        {
            CurrentUser = _user;
        }
        public static void Logout(User _user)
        {
            CurrentUser = null;
        }
    }
}
