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

namespace donely_Inspilab.Pages
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
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            //check een van velden leeg
            if (txtName.Text.Trim() == "" || txtEmail.Text.Trim() == "" || txtTelnr.Text.Trim() == "" || txtPassword.Text.Trim() == "" || txtConfirmPassword.Text.Trim() == "")
            {
                MessageBox.Show("Not all required fields are filled in", "Incomplete Registration", MessageBoxButton.OK);
                return;
            }

            UserRegistration newUser = new(txtName.Text, txtEmail.Text, txtTelnr.Text, txtPassword.Text, txtConfirmPassword.Text);
            newUser.ValidateFields();
            MessageBox.Show(newUser.Email);



            
        }
    }
}
