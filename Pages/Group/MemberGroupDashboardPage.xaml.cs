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
using donely_Inspilab.Enum;
using donely_Inspilab.Pages.Shop;

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
            this.Loaded += MemberGroupDashboardPage_Loaded;
            LoadItems();
        }

        private void MemberGroupDashboardPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Reload group members from the database
            var db = new Database();
            GroupState.LoadedGroup.Members = db.GetGroupMembers(GroupState.LoadedGroup.Id);

            LoadItems();
        }

        private void LoadItems()
        {


            //groupmember splitsen in current member en rest
            CurrentMember = (GroupMember)GroupState.LoadedGroup.Members.FirstOrDefault(t => t.UserId == SessionManager.GetCurrentUserID());
            OtherMembers = GroupState.LoadedGroup.Members.Where(t => t.UserId != SessionManager.GetCurrentUserID()).ToList(); //Huidige user uit de memberlijst halen

            //taken laden
            CurrentMember = TaskService.LoadAndAssignTaskInstances(CurrentMember);

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
            var task = (sender as Button)?.DataContext as TaskInstance;
            SetTaskStatus(task, TaskProgress.Success);
        }

        private void FailTask_Click(object sender, RoutedEventArgs e)
        {
            var task = (sender as Button)?.DataContext as TaskInstance;
            SetTaskStatus(task, TaskProgress.Failure);
        }

        private void SetTaskStatus(TaskInstance task, TaskProgress status)
        {
            try
            {
                task.CompletionDate = DateTime.Now;
                string message = "";
                if (status == TaskProgress.Success)
                {
                    if (task.DeadlineDateOnly < DateOnly.FromDateTime((DateTime)task.CompletionDate)) //AUTOFAIL DEADLINE (backup measure)
                    {
                        task.Status = TaskProgress.Failure; //autofail als deadline toch vervallen op moment van indienen.
                        message = "Deadline has passed at time of handing in, Task Failed!";
                    }
                    else if (task.Task.RequiresValidation) //TO PENDING
                    {
                        task.Status = TaskProgress.Pending;
                        message = $"Task successfully handed in, please wait for {GroupState.LoadedGroup.Owner.Name} to confirm!";
                    }
                    else // SUCCESS
                    {
                        task.Status = status;
                        message = $"Task done successfully! Here are {task.Task.Reward} coins!";
                        CurrentMember.Currency = GroupMemberService.AddCurrency(CurrentMember, task.Task.Reward); //Doet zowel update in database als in klasse
                    }
                }
                else
                {//FAILURE
                    task.Status = status;
                }
                ;
                TaskService.UpdateTaskInstance(task); //update voor database
                CurrentMember.UpdateTaskStatus(task); //update voor klasse

                lblCurrency.Content = CurrentMember.Currency;
                //reset listview
                lsvMemberTasks.ItemsSource = CurrentMember.ActiveTaskList;
                lsvMemberTasks.Items.Refresh();

                if (task.Status != TaskProgress.Failure)
                    MessageBox.Show(message, "Result", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Task Failed. You'll get 'em next time!", "Result", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Unexpected Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }
        private void GoToShop(object sender, RoutedEventArgs e)
        {
            // Navigate to the ShopPage
            this.NavigationService.Navigate(new MemberShopPage());
        }

        //leave group
        private void LeaveGroup_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to leave this group?", "Leave Group", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                int userId = SessionManager.GetCurrentUserID();
                int groupId = GroupState.LoadedGroup.Id;

                Database db = new Database();
                bool success = db.LeaveGroup(userId, groupId);

                if (success)
                {
                    MessageBox.Show("You have left the group.", "Left Group", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavService.ToHomePage();
                }
                else
                {
                    MessageBox.Show("Something went wrong. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


    }
}
