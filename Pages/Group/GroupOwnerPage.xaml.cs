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
            lblGroupName.Content = GroupState.LoadedGroup.Name;
            imgGroupImage.Source = GroupState.LoadedGroup.ImageSource;
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
            NavService.ToTaskLibraryPage();
        }

        private void ToHomePage_Click(object sender, RoutedEventArgs e)
        {
            NavService.ToHomePage();
        }

        private void KickMember_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lsvMembersOverview.SelectedItem == null)
                {
                    MessageBox.Show("Please select a member first!", "Nothing selected", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                GroupMember selectedMember = lsvMembersOverview.SelectedItem as GroupMember;
                var answer = MessageBox.Show($"Are you sure you want to kick {selectedMember.User.Name} from the group?", "Kicking member", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (answer == MessageBoxResult.Yes)
                {
                    GroupMemberService.KickMember(selectedMember);
                    GroupState.LoadedGroup.Members.Remove(selectedMember); //reloading full list, just to be sure
                    lsvMembersOverview.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Something went wrong - {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

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

        private void DeleteGroup_Click(object sender, RoutedEventArgs e)
        {
            try { 
            var answer = MessageBox.Show("Are you sure?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (answer == MessageBoxResult.Yes) {
                    DeleteGroupWindow deleteWindow = new();
                    deleteWindow.ShowDialog();
                    if ((bool)deleteWindow.DialogResult)
                    {
                        MessageBox.Show("Group successfully deleted");
                        NavService.ToHomePage();
                    }
                    else {
                        MessageBox.Show("Group not deleted");
                    }
                }

            }
            catch( Exception ex)
            {
                MessageBox.Show("Something unexpected went wrong", "error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ToManageShopPage_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("TODO SET UP SHOP PAGE!!!"); //TODO
        }
    }
}
