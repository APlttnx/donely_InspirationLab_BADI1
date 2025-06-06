﻿using donely_Inspilab.Pages;
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
using donely_Inspilab.Pages.Shop;

namespace donely_Inspilab.Classes
{
    public static class NavService
        //Klasse voor het faciliteren van navigatie tussen de verschillende pagina's, plus aantal voorwaarden indien nodig (bv voor HomePage checken of ingelogd als backup)
    {
        public static void ToHomePage()
        {
            if (SessionManager.IsLoggedIn)
            {
                if (SessionManager.CurrentUser.IsAdmin)
                {
                    App.MainFrame.Navigate(new AdminDashboard());
                    return;
                }
                GroupState.ClearGroup(); //Elke keer als homepage geladen wordt, wordt de geladen group leeggemaakt --> Via home kan je terug een groep selecteren
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
        {
            App.MainFrame.Navigate(new TaskLibraryPage());
        }

        // Geen MemberState, dus voor deze wordt de member als parameter meegegeven zodat deze kan geladen worden in de respectievelijke pagina
        public static void ToManageMemberTasksPage(GroupMember member)
        {
            App.MainFrame.Navigate(new ManageMemberTasksPage(member));

        }
        public static void ToAssignTaskPage(GroupMember member)
        {
            App.MainFrame.Navigate(new AssignTaskPage(member));
        }


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
        }

        //SHOP
        public static void ToMemberShopPage()
        {
            App.MainFrame.Navigate(new MemberShopPage());
        }
        public static void ToOwnerManageShopPage()
        {
            App.MainFrame.Navigate(new OwnerManageShop());
        }

        // Add other navigation methods as needed...
    }

}
