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
    /// Interaction logic for GroupDashboardPage.xaml
    /// </summary>
    public partial class MemberGroupDashboardPage : Page
    {

        private GroupMember CurrentMember;
        private List<GroupMember> OtherMembers;
        public MemberGroupDashboardPage()
        {
            InitializeComponent();
            LoadItems();
            
        }


        private void LoadItems()
        {


            //groupmember splitsen in current member en rest
            CurrentMember = (GroupMember)GroupState.LoadedGroup.Members.FirstOrDefault(t => t.UserId == SessionManager.GetCurrentUserID());
            OtherMembers = GroupState.LoadedGroup.Members.Where(t => t.UserId != SessionManager.GetCurrentUserID()).ToList(); //Huidige user uit de memberlijst halen

            //taken laden
            CurrentMember = TaskService.LoadTaskInstances(CurrentMember);

            //Listviews en labels invullen
            lblGroupName.Content = GroupState.LoadedGroup.Name;
            imgGroupImage.Source = GroupState.LoadedGroup.ImageSource;
            lsvMembersOverview.ItemsSource = OtherMembers;
            lsvMemberTasks.ItemsSource = CurrentMember.ActiveTaskList;
            lblCurrency.Content = CurrentMember.Currency;
        }
        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            NavService.ToHomePage();
        }

        private void SucceedTask_Click(object sender, RoutedEventArgs e)
        {

        }
        private void FailTask_Click(object sender, RoutedEventArgs e)
        {

        }


    }
}
