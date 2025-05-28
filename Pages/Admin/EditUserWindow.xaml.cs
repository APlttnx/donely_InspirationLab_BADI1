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

namespace donely_Inspilab.Pages.Admin
{
    public partial class EditUserWindow : Window
    {
        private User _user;

        public EditUserWindow(User user)
        {
            InitializeComponent();
            _user = user;

            TxtName.Text = _user.Name;
            TxtEmail.Text = _user.Email;
            TxtPhone.Text = _user.TelephoneNumber;
        }

        public EditUserWindow()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ChangeUser_Click(object sender, RoutedEventArgs e)
        {
            _user.Name = TxtName.Text;
            _user.Email = TxtEmail.Text;
            _user.TelephoneNumber = TxtPhone.Text;

            try
            {
                Database db = new();

                db.UpdateUser(_user); //kleine aanpassing gemaakt --> Nieuwe methode in UserService, centraal punt voor alle updates van de fields voor User

                MessageBox.Show("User updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update failed: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



    }
}