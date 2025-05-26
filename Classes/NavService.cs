using donely_Inspilab.Pages;
using donely_Inspilab.Pages.auth;
using donely_Inspilab.Pages.Settings;
using donely_Inspilab.Pages.Group;

using donely_Inspilab.Pages.Admin;
using donely_Inspilab.Pages.Task;
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
                GroupState.ClearGroup(); //Elke keer als homepage geladen wordt, wordt de geladen group leeggemaakt --> Via home kan je terug een groep selecteren


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
                // Naar login navigeren als niet ingelogd
                ToLoginPage();
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
            App.MainFrame.Navigate(new MemberGroupDashboardPage());
        }
        public static void ToGroupOwnerPage()
        {
            App.MainFrame.Navigate(new GroupOwnerPage());
        }
        public static void ToGroupCreationPage()
        {
            App.MainFrame.Navigate(new GroupCreationPage());
        }
        public static void ToTaskLibraryPage()
        //ADMIN
        public static void ToAdminDashboard()
        {
            App.MainFrame.Navigate(new AdminDashboard());
        }
        public static void ToAdminOverview()
        {
            App.MainFrame.Navigate(new UserOverviewPage());
        }
        public static void ToAdminCreateUserPage()
        {
            App.MainFrame.Navigate(new CreateUserPage());

        public static void ToManageTasksPage()
        {
            App.MainFrame.Navigate(new TaskLibraryPage());
        }
        public static void ToManageMemberTasksPage(GroupMember member)
        {
            App.MainFrame.Navigate(new ManageMemberTasksPage(member));

        }
        public static void ToAssignTaskPage(GroupMember member)
        {
            App.MainFrame.Navigate(new AssignTaskPage(member));
        }

        // Add other navigation methods as needed...
    }

}
