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
                if (currentUser != null) // dubbelchecken dat de login check succesvol was
                {
                    SessionManager.Login(currentUser);

                    TaskService.AutoFailExpiredTasksGlobal(); // Bij login --> Run global AutoFailer (alle expired tasks worden dan automatisch gemarkeerd als Failed. Onze db is klein genoeg dat dit zo geen problemen oplevert, maar zou later een robustere oplossing nodig hebben
                    TaskService.AutoReassignRecurringTasks(); //same als hierboven. Recurrente opdrachten worden automatisch herhaald als alle voorwaarden voldaan
                    // Voor grotere db --> mss ook best Async?
                    NavService.ToHomePage();

                    System.Diagnostics.Debug.WriteLine("Admin? " + currentUser.IsAdmin);

                    if (currentUser.IsAdmin) //Check of huidige gebruiker een admin is --> naar andere pagina navigeren
                        NavService.ToAdminDashboard();
                    else
                        NavService.ToHomePage();

                }
                else
                {
                    throw new ArgumentException("Wrong password or email");
                }
            }
            catch (ArgumentException ex) //Errormessage display
            {
                lblError.Content = ex.Message;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong - " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void GoBack(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }


    }
}
