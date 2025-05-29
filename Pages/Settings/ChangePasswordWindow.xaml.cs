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
using System.Windows.Shapes;

namespace donely_Inspilab.Pages.Settings
{
    public partial class ChangePasswordWindow : Window
    {
        public ChangePasswordWindow()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            string current = PwdCurrent.Password;
            string newPwd = PwdNew.Password;
            string confirm = PwdConfirm.Password;

            if (string.IsNullOrWhiteSpace(current) || string.IsNullOrWhiteSpace(newPwd))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (newPwd != confirm)
            {
                MessageBox.Show("New passwords do not match.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Get current user ID
                int userId = SessionManager.GetCurrentUserID();
                Database db = new Database();

                // 1. Verify the current password
                var credentials = db.GetUserCredentialsByEmail(SessionManager.CurrentUser.Email);
                if (!BCrypt.Net.BCrypt.Verify(current, credentials.HashedPassword))
                {
                    MessageBox.Show("Current password is incorrect.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // 2. Hash the new password
                string newHashedPassword = BCrypt.Net.BCrypt.HashPassword(newPwd);

                // 3. Update password in DB
                bool updated = db.UpdateUserPassword(userId, newHashedPassword);

                if (updated)
                {
                    MessageBox.Show("Password changed successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to change password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}