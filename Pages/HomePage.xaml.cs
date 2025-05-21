using donely_Inspilab.Classes;
using donely_Inspilab.Pages.Group;
using donely_Inspilab.Pages.Settings;

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


namespace donely_Inspilab.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        private List<Classes.Group> GroupList { get; set; } = new(); //dient voor dynamische upload ListView
        private List<Classes.Group> OwnedGroupList { get; set; } = new(); //dient voor dynamische upload ListView
        public HomePage()
        {
            InitializeComponent();
            LoadGroups(); //Inladen groepen via database en in listviews plaatsen
        }

        private void LoadGroups()
        {
            try
            {
                GroupList.Clear();
                GroupList = GroupService.GetOverviewGroups((int)SessionManager.GetCurrentUserID());
                lsvGroupOverview.ItemsSource = GroupList;
                OwnedGroupList.Clear();
                OwnedGroupList = GroupService.GetOverviewOwnGroups(SessionManager.CurrentUser);
                lsvOwnedGroupsOverview.ItemsSource = OwnedGroupList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Something went wrong with the database", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        
        private void ToGroupCreation(object sender, RoutedEventArgs e)
        {
            NavService.ToGroupCreationPage();
        }

        private void ToJoinGroup(object sender, RoutedEventArgs e)
        {
            JoinGroupWindow joinGroupWindow = new();
            var dialogResult = joinGroupWindow.ShowDialog();
            if (dialogResult == true){
                MessageBox.Show("Group successfully joined!", "Join Success", MessageBoxButton.OK);
            }
        }

    }
}
