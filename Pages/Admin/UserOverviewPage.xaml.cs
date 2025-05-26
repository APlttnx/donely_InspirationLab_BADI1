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

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            NavService.ToAdminEditUserPage();
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            NavService.ToAdminDeleteUserPage();
        }

    }
}
