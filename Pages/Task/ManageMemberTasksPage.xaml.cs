using donely_Inspilab.Classes;
using donely_Inspilab.Enum;
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

namespace donely_Inspilab.Pages.Task
{
    /// <summary>
    /// Interaction logic for ManageMemberTasks.xaml
    /// </summary>
    public partial class ManageMemberTasksPage : Page
    {
        private GroupMember Member { get; set; }

        public ManageMemberTasksPage(GroupMember member)
        {
            InitializeComponent();
            Member = member;
            lblMemberName.Content = $"Tasks of {Member.User.Name}";
            LoadTaskInstances();
        }
        private void LoadTaskInstances()
        {
            Member = TaskService.LoadAndAssignTaskInstances(Member);
            lsvActiveTasks.ItemsSource =  Member.ActiveTaskList;
            lsvPendingTasks.ItemsSource = Member.PendingTaskList;
            lsvCompletedTasks.ItemsSource = Member.CompletedTaskList;
        }

        private void GoBack_click(object sender, RoutedEventArgs e)
        {
            NavService.ToGroupOwnerPage();
        }

        private void GiveTask_Click(object sender, RoutedEventArgs e)
        {
            NavService.ToAssignTaskPage(Member);
        }
        private void SucceedTask_Click(object sender, RoutedEventArgs e)
        {
            var task = (sender as Button)?.DataContext as TaskInstance;
            SetTaskStatus(task, TaskProgress.Success);

            //// The DataContext of the Button is the Member bound in the DataTemplate
            //GroupMember member = button.DataContext as GroupMember;
            //if (member == null) return;

        }
        private void FailPendingTask_Click(object sender, RoutedEventArgs e)
        {
            var task = (sender as Button)?.DataContext as TaskInstance;
            SetTaskStatus(task, TaskProgress.Failure);

            //// The DataContext of the Button is the Member bound in the DataTemplate
            //GroupMember member = button.DataContext as GroupMember;
            //if (member == null) return;
        }

        private void SetTaskStatus(TaskInstance task, TaskProgress status)
        {
            task.Status = status;
            TaskService.UpdateTaskInstance(task); //update voor database
            Member.UpdateTaskStatus(task); //update voor klasse
            if (status == TaskProgress.Success)
            {
                Member.Currency = GroupMemberService.AddCurrency(Member, task.Task.Reward);
            }
             //Doet zowel update in database als in klasse
            //reset listview
            LoadTaskInstances();
        }
    }
}
