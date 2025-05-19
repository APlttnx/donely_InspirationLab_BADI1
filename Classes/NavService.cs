using donely_Inspilab.Pages;
using donely_Inspilab.Pages.auth;
using donely_Inspilab.Pages.Settings;
using donely_Inspilab.Pages.Group;
//using donely_Inspilab.Pages.Task;
//using donely_Inspilab.Pages.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace donely_Inspilab.Classes
{
    public static class NavService
    {
        public static void ToHomePage()
        {
            if (SessionManager.IsLoggedIn)
            {
                /* Optie om homepagina te cachen en zo niet alles elke keer te herladen. Vond zelf achteraf dat dit misschien niet nodig was op deze schaal.
                // If HomePage is already cached, navigate to it
                //App.HomePage ??= new HomePage();

                // Navigate to the cached HomePage
                //App.MainFrame.Navigate(App.HomePage);
                */
                App.MainFrame.Navigate(new HomePage());
            }
            else
            {
                // Navigate to a login or welcome page if the user is not logged in
                App.MainFrame.Navigate(new LoginPage());
            }
        }

        public static void ToWelcomePage()
        {
            App.MainFrame.Navigate(new WelcomePage());
        }
        public static void ToLoginPage()
        {
            App.MainFrame.Navigate(new LoginPage());
        }
        public static void ToRegisterPage()
        {
            App.MainFrame.Navigate(new RegisterPage());
        }
        public static void ToSettingsPage()
        {
            App.MainFrame.Navigate(new SettingsPage());
        }
        //GROUPS
        public static void ToGroupPage()
        {
            App.MainFrame.Navigate(new GroupDashboardPage());
        }
        public static void ToGroupCreationPage()
        {
            App.MainFrame.Navigate(new GroupCreationPage());
        }
        public static void ToGroupOwnerPage()
        {
            App.MainFrame.Navigate(new GroupOwnerPage());
        }



        // Add other navigation methods as needed...
    }

}
