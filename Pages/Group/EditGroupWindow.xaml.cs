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

namespace donely_Inspilab.Pages.Group
{
    /// <summary>
    /// Interaction logic for EditGroupWindow.xaml
    /// </summary>
    public partial class EditGroupWindow : Window
    {
        private string _fileName = "default.png";
        public EditGroupWindow()
        {
            InitializeComponent();
            LoadFields();

        }
        private void LoadFields()
        {
            //Huidige gegevens van de geladen groep tonen voordat je edit
            txtName.Text = GroupState.LoadedGroup.Name;
            _fileName = GroupState.LoadedGroup.ImageLink;
            UploadedImage.Source = new BitmapImage(new Uri($"/Assets/GroupImages/{_fileName}", UriKind.Relative));
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void EditGroup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                if (string.IsNullOrWhiteSpace(txtName.Text))
                    throw new ArgumentException("Group has to have a name!");
                //Niets aangepast
                if (txtName.Text == GroupState.LoadedGroup.Name && _fileName == GroupState.LoadedGroup.ImageLink)
                {
                    this.DialogResult = false; 
                    this.Close();
                }
                else
                {
                    //TODO: Group Edit via database
                    this.DialogResult = true;
                }
            }

            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Invalid edit", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            
        }

        private void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = ImageUploader.UploadImage("Assets/GroupImages");
                if (result != null)
                {
                    UploadedImage.Source = result.Value.image;
                    _fileName = result.Value.fileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to upload image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
