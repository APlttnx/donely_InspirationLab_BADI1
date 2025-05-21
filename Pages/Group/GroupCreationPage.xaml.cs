using donely_Inspilab.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Xml.Linq;

namespace donely_Inspilab.Pages.Group
{
    /// <summary>
    /// Interaction logic for GroupCreationPage.xaml
    /// </summary>
    /// 
    
    public partial class GroupCreationPage : Page
    {
        private ObservableCollection<ShopItem> ShopList { get; set; } = new(); //dient voor dynamische upload ListView
        private string _fileName = "default.png";
        public GroupCreationPage()
        {
            InitializeComponent();
            PrepareShopList();
            UploadedImage.Source = new BitmapImage(new Uri("Assets/Images/placeholder.jpg", UriKind.Relative));
        }


        private void PrepareShopList()
        {
            lsvShopItems.ItemsSource = ShopList;
        }

        private void AddReward_Click(object sender, RoutedEventArgs e)
        {
            AddRewardWindow rewardWindow = new();
            var dialogResult = rewardWindow.ShowDialog();

            if (dialogResult == true && rewardWindow.NewShopItem != null)
            {
                ShopList.Add(rewardWindow.NewShopItem);
            }
        }

        private void GoToHome(object sender, RoutedEventArgs e)
        {
            NavService.ToHomePage();
        }

        private void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";

            if (dialog.ShowDialog() == true)
            {
                string sourcePath = dialog.FileName;
                string fileName = System.IO.Path.GetFileName(sourcePath); //System.IO specifiëren, anders error door name conflict met shapes

                string imagesFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets/GroupImages");

                try
                {
                    if (!Directory.Exists(imagesFolder))
                        Directory.CreateDirectory(imagesFolder);

                    string destinationPath = System.IO.Path.Combine(imagesFolder, fileName);

                    File.Copy(sourcePath, destinationPath, overwrite: true);

                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(destinationPath, UriKind.Absolute);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();

                    UploadedImage.Source = bitmap;

                    _fileName = fileName;
                    // Save _fileName somewhere persistent here if needed
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to upload image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }




        private void CreateNewGroup_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text)) throw new ArgumentException("Please fill in the required fields.");
                Classes.Group newGroup = GroupService.CreateGroup(txtName.Text, _fileName, ShopList.ToList());
                if (ShopList.Count != 0)
                {
                    ShopItemService.InsertShopItems(newGroup.ShopItems, newGroup.Id);
                }
                MessageBox.Show($"Group {newGroup.Name} successfully created", "Registration Failed", MessageBoxButton.OK);
                NavService.ToGroupPage();

            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error "+ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}
