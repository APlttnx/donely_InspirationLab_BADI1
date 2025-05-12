using donely_Inspilab.Classes;
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

namespace donely_Inspilab.Pages.auth
{

    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        private void ReturnToWelcome(object sender, RoutedEventArgs e)
        {
            NavService.ToWelcomePage();    
        }
        private void btnLogin_click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtEmail.Text == "" || txtPassword.Password == "")
                    throw new ArgumentException("Please fill in the required fields");

                User currentUser = UserService.Login(txtEmail.Text, txtPassword.Password);
                if (currentUser != null)
                {
                    SessionManager.Login(currentUser);
                    NavService.ToHomePage();
                }
                else
                {
                    throw new ArgumentException("Wrong password or email");
                }
            }
            catch (ArgumentException ex)
            {
                lblError.Content = ex.Message;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong - " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
