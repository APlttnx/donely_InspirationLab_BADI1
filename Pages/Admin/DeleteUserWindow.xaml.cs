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
    public partial class DeleteUserWindow : Window
    {
        private User _userToDelete;

        public DeleteUserWindow(User user)
        {
            InitializeComponent();
            _userToDelete = user;
            TxtConfirmation.Text = $"Are you sure you want to delete [{_userToDelete.Name}]?";
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Database db = new();
                db.DeleteUser(_userToDelete.Id.Value);

                MessageBox.Show("User deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch
            {
                MessageBox.Show("Error deleting user.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}