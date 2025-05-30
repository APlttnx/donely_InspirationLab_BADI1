using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using donely_Inspilab.Classes;

namespace donely_Inspilab.Pages
{

    public partial class HeaderControl : UserControl
    {
        public HeaderControl()
        {
            InitializeComponent();
            SessionManager.LoginStatusChanged += (_, __) => UpdateUI(); //Subscription voor event uitloggen => knoppen wisselen
            SessionManager.ProfileUpdated += UpdateUI;
            UpdateUI();
        }

        public void UpdateUI() //Dit wisselt de knoppen bovenaan naargelang de gebruiker is ingelogd of niet. En wordt geactiveerd door o.a. de LoginStatusChanged en ProfileUpdated events.
        {
            imgProfilePic.Source = SessionManager.IsLoggedIn ? SessionManager.CurrentUser.ImageSource : null; //profielfoto bovenaan
            GuestButtons.Visibility = SessionManager.IsLoggedIn ? Visibility.Collapsed : Visibility.Visible; 
            UserButtons.Visibility = SessionManager.IsLoggedIn ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            NavService.ToRegisterPage();
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            NavService.ToLoginPage();
        }
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            NavService.ToSettingsPage();
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            SessionManager.Logout();
        }
    }
}
