using donely_Inspilab.Classes;
using donely_Inspilab.Pages.auth;
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

namespace donely_Inspilab.Pages.Settings
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }


        private void GoToChangePfp(object sender, RoutedEventArgs e)
        {
            EditProfilePictureWindow editPfp = new EditProfilePictureWindow();
            bool answer = (bool)editPfp.ShowDialog();

            if (answer)
                MessageBox.Show("Profile picture successfully changed", "Success", MessageBoxButton.OK);
        }

        private void GoToChangePassword(object sender, RoutedEventArgs e)
        {
            ChangePasswordWindow pwdWindow = new ChangePasswordWindow();
            bool? result = pwdWindow.ShowDialog();
            if (result == true)
            {
                MessageBox.Show("Password changed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void GoToDeleteAccount(object sender, RoutedEventArgs e)
        {
            if (!SessionManager.IsLoggedIn) //check of er een gebruiker is ingelogd om te deleten
            {
                MessageBox.Show("Please log in first:", "User not logged in", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                NavService.ToLoginPage();
                return;
            }
            //waarschuwing
            MessageBoxResult answer = MessageBox.Show("Are you certain? this decision is permanent!", "Are you certain?", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (answer == MessageBoxResult.Yes)
            {
                bool success = UserService.DeleteUser(SessionManager.CurrentUser.Id.Value);
                if (success)
                {
                    MessageBox.Show(SessionManager.CurrentUser.Name + " has been deleted", "User removed", MessageBoxButton.OK, MessageBoxImage.Information);
                    App.LogoutAndReset();
                }
            }
        }
        private void GoBack(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }


    }
}
