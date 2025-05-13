using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using donely_Inspilab.Classes;

namespace donely_Inspilab.Pages.Group
{
    /// <summary>
    /// Interaction logic for GroupCreationPage.xaml
    /// </summary>
    /// 
    
    public partial class GroupCreationPage : Page
    {
        private ObservableCollection<ShopItem> ShopList { get; set; } = new(); //dient voor dynamische upload ListView

        public GroupCreationPage()
        {
            InitializeComponent();
            PrepareShopList();
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

        }
    }
}
