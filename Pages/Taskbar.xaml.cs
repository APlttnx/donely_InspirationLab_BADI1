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
    /// <summary>
    /// Interaction logic for Taskbar.xaml
    /// </summary>
    public partial class Taskbar : UserControl
    {
        public Taskbar()
        {
            InitializeComponent();
            UpdateUI();
        }

        public void UpdateUI()
        {
            GuestButtons.Visibility = SessionManager.IsLoggedIn ? Visibility.Collapsed : Visibility.Visible; 
            UserButtons.Visibility = SessionManager.IsLoggedIn ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Settings_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
