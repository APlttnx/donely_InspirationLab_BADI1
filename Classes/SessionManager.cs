using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace donely_Inspilab.Classes
{
    //Houdt huidig ingelogde gebruiker bij zodat deze kan gebruikt worden doorheen de app om bv Groupen op te laden, of te checken of er een gebruiker is ingelogd
    public static class SessionManager
    {
        public static User? CurrentUser { get; private set; }
        public static bool IsLoggedIn => CurrentUser != null;

        public static event EventHandler? LoginStatusChanged; //Event voor als login/logout --> dient bv om knoppen in HeaderControl/FooterControl aan te passen

        public static void Login(User _user) //Ingelogde user aan session toevoegen + login event
        {
            CurrentUser = _user;
            LoginStatusChanged?.Invoke(null, EventArgs.Empty);
        }

        public static void UpdateUser(User _user)
        {
            CurrentUser = _user;
        }


        public static void Logout()
        {
            CurrentUser = null;
            LoginStatusChanged?.Invoke(null, EventArgs.Empty);
            NavService.ToWelcomePage();
        }
        
        public static int GetCurrentUserID() //Rechtstreeks huidige ID opvragen, als niet mogelijk --> auto navigate naar login pagina
        {
            if (IsLoggedIn)
            {
                return (int)CurrentUser.Id;
            }
            else
            {
                NavService.ToLoginPage();
                return -1;
            }
            
        }
        public static event Action ProfileUpdated; //Nog een event voor als het profiel wordt geüpdatet bv met een nieuwe foto

        public static void RaiseProfileUpdated()
        {
            ProfileUpdated?.Invoke();
        }
    }
}
