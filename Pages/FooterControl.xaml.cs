using System.Windows;
using System.Windows.Controls;
using donely_Inspilab.Classes;

namespace donely_Inspilab.Pages
{
    public partial class FooterControl : UserControl
    {
        public FooterControl()
        {
            InitializeComponent();
            SessionManager.LoginStatusChanged += (_, __) => UpdateUI(); // Subscribe to login status changes
            UpdateUI(); // Set initial state
        }

        private void UpdateUI()
        {
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
            SessionManager.Logout(); // Triggers LoginStatusChanged event
        }
    }
}
