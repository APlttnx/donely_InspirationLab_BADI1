using donely_Inspilab.Classes;
using donely_Inspilab.Methods;
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
using System.Xml.Linq;

namespace donely_Inspilab.Pages.Settings
{
    /// <summary>
    /// Interaction logic for EditProfilePicture.xaml
    /// </summary>
    public partial class EditProfilePictureWindow : Window
    {
        private string _fileName;
        private User _user;
        public EditProfilePictureWindow()
        {
            InitializeComponent();
            _user = SessionManager.CurrentUser;
            imgProfilePicture.Source = _user.ImageSource;
            _fileName = _user.ProfilePicture;
        }
        

        private void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = ImageUploader.UploadImage("Assets/ProfilePictures");
                if (result != null)
                {
                    imgProfilePicture.Source = result.Value.image;
                    _fileName = result.Value.fileName;
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to upload image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Nothing changed
                if (_fileName == _user.ProfilePicture)
                {
                    this.DialogResult = false;
                    this.Close();
                }
                else
                {
                    _user.ProfilePicture = _fileName;
                    UserService.UpdateUser(_user);
                    SessionManager.CurrentUser.ProfilePicture = _fileName;
                    SessionManager.RaiseProfileUpdated();
                    this.DialogResult = true;
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Invalid edit", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
