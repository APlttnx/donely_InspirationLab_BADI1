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
using donely_Inspilab.Classes;

namespace donely_Inspilab.Pages.Group
{
    /// <summary>
    /// Interaction logic for GroupOwnerPage.xaml
    /// </summary>
    public partial class GroupOwnerPage : Page
    {
        public GroupOwnerPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            if (!GroupState.IsGroupLoaded)
            {
                //failsafe
                MessageBox.Show("No group selected");
                NavService.ToHomePage();
                return;
            }
            lsvMembersOverview.ItemsSource = GroupState.LoadedGroup.Members;
        }

        private void ToEditGroupWindow_Click(object sender, RoutedEventArgs e)
        {
            EditGroupWindow editGroupWindow = new();
            bool result = (bool)editGroupWindow.ShowDialog();

        }

        private void ToInviteCodeWindow_Click(object sender, RoutedEventArgs e)
        {
            ShowInviteCodeWindow codeWindow = new();
            codeWindow.Show();
        }

        private void ToCreatedTaskOverviewPage_Click(object sender, RoutedEventArgs e)
        {
            NavService.ToManageTasksPage();
        }

        private void ToHomePage_Click(object sender, RoutedEventArgs e)
        {
            NavService.ToHomePage();
        }

        private void KickMember_Click(object sender, RoutedEventArgs e)
        {
            //TODO: kick member --> Delete from user_group table (MessageBox "You Sure?" -> GroupMemberService -> db.Delete (zeker zijn dat dit ook gebruiker verwijdert in relevante tabellen
        }

        
        private void ManageTasksButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            // The DataContext of the Button is the Member bound in the DataTemplate
            GroupMember member = button.DataContext as GroupMember;
            if (member == null) return;

            // Navigate or do whatever you need with this member
            NavService.ToManageMemberTasksPage(member);
        }
    }
}
