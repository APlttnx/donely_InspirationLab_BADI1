using donely_Inspilab.Classes;
using donely_Inspilab.Pages;
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
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }
        private void btnRegister_click(object sender, RoutedEventArgs e)
        {
            //check een van velden leeg
            if (txtName.Text.Trim() == "" || txtEmail.Text.Trim() == "" || txtTelnr.Text.Trim() == "" || txtPassword.Password.Trim() == "" || txtConfirmPassword.Password.Trim() == "")
            {
                MessageBox.Show("Not all required fields are filled in", "Incomplete Registration", MessageBoxButton.OK);
                return;
            }

            //UserRegistration newUser = new(txtName.Text, txtEmail.Text, txtTelnr.Text, txtPassword.Text, txtConfirmPassword.Text);
            //newUser.ValidateFields();
            //MessageBox.Show(newUser.Email);

            //Als succesvol => momenteel rerouten naar login pagina, kan ook rechtstreeks naar Home/Dashboard of indien toegevoegd ConfirmEmailPage (bonus)
            App.MainFrame.Navigate(new LoginPage());

        }

        private void ReturnToWelcome(object sender, RoutedEventArgs e)
        {
            App.MainFrame.Navigate(new WelcomePage());
        }
    }
}
