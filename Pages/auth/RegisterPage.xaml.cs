﻿using donely_Inspilab.Classes;
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
            try
            {
                //check een van velden leeg
                if (txtName.Text.Trim() == "" || txtEmail.Text.Trim() == "" || txtTelnr.Text.Trim() == "" || txtPassword.Password.Trim() == "" || txtConfirmPassword.Password.Trim() == "")
                    throw new ArgumentException("Not all fields are filled in");
       
                User newUser = UserService.Register(txtName.Text, txtEmail.Text, txtTelnr.Text, txtPassword.Password, txtConfirmPassword.Password);

                MessageBox.Show($"{newUser.Name} has been added", "Registration Success", MessageBoxButton.OK);

                //Als succesvol => momenteel rerouten naar login pagina, kan ook rechtstreeks naar Home/Dashboard of indien toegevoegd ConfirmEmailPage (bonus)
                NavService.ToHomePage();
            }
            
            catch ( ArgumentException argument) //ArgumentException heeft meestal een custom message 
            {
                MessageBox.Show(argument.Message, "Registration Failed", MessageBoxButton.OK); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong - "+ex.Message,"Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

    }
}
