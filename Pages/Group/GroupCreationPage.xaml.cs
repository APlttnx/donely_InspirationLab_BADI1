using donely_Inspilab.Classes;
using donely_Inspilab.Methods;
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
            UploadedImage.Source = new BitmapImage(new Uri($"/Assets/GroupImages/{_fileName}", UriKind.Relative));
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
            try
            {
                var result = ImageUploader.UploadImage("Assets/GroupImages");
                if (result != null)
                {
                    UploadedImage.Source = result.Value.image;
                    _fileName = result.Value.fileName;
                    // Save _fileName if needed
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to upload image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                GroupState.LoadGroup(newGroup);
                NavService.ToGroupOwnerPage();

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
