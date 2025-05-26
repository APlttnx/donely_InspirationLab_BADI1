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
    public partial class UserOverviewPage : Page
    {
        public UserOverviewPage()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            Database db = new();
            List<User> users = db.GetAllUsers(); // You need to implement this method
            UserDataGrid.ItemsSource = users;
        }

        private void CreateUser_Click(object sender, RoutedEventArgs e)
        {
            NavService.ToAdminCreateUserPage();
        }

        private void ChangeUser_Click(object sender, RoutedEventArgs e)
        {
            if (UserDataGrid.SelectedItem is User selectedUser)
            {
                EditUserWindow editWindow = new(selectedUser); // pass user
                bool? result = editWindow.ShowDialog();

                if (result == true)
                {
                    LoadUserList(); // reload table after update
                }
            }
            else
            {
                MessageBox.Show("Please select a user to change.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (UserDataGrid.SelectedItem is User selectedUser)
            {
                DeleteUserWindow deleteWindow = new(selectedUser);
                bool? result = deleteWindow.ShowDialog();

                if (result == true)
                {
                    LoadUserList(); // refresh the table
                }
            }
            else
            {
                MessageBox.Show("Please select a user to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void LoadUserList()
        {
            Database db = new();
            UserDataGrid.ItemsSource = db.GetAllUsers(); // Assumes you have a method to get all users
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null && NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }


    }
}
