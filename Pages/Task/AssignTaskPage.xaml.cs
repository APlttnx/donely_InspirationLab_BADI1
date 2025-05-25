using donely_Inspilab.Classes;
using donely_Inspilab.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace donely_Inspilab.Pages.Task
{
    /// <summary>
    /// Interaction logic for AssignTaskWindow.xaml
    /// </summary>
    public partial class AssignTaskPage : Page
    {

        private List<Classes.Task> taskList = [];

        public TaskInstance NewAssignedTask { get; private set; }

        private GroupMember Member { get; set; }


        public AssignTaskPage(GroupMember chosenMember)
        {
            InitializeComponent();
            Member = chosenMember;
            cmbFrequency.ItemsSource = System.Enum.GetValues(typeof(TaskFrequency));
            cmbFrequency.SelectedIndex = 0; // default
            LoadLibrary();
        }


        private void LoadLibrary()
        {
            try
            {
                taskList = TaskService.GetGroupDefinitions(GroupState.LoadedGroup.Id);
                foreach (var task in taskList)
                {
                    if (task.IsActive == false)
                        taskList.Remove(task); //inactieve taken niet laten zien
                }
                lsvTaskLibrary.ItemsSource = taskList;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong loading tasks");
            }
        }


        private void CreateAssignTask_Click(object sender, RoutedEventArgs e) //Voor nieuwe taak aan te maken in assign window
        {
            try
            {
                //Check req fields filled in
                if (string.IsNullOrWhiteSpace(txtTaskName.Text) || dpDeadlineNew.SelectedDate == null) throw new ArgumentException("Not all required fields are filled in!");
                Classes.Task newTask = TaskService.CreateTask(txtTaskName.Text, txtDescription.Text, Convert.ToInt32(txtReward.Text), (TaskFrequency)cmbFrequency.SelectedItem, (bool)cbValidation.IsChecked, GroupState.LoadedGroup.Id);
                if (newTask != null)
                {
                    NewAssignedTask = TaskService.CreateTaskInstance(newTask, dpDeadlineNew.SelectedDate.Value, Member);
                    MessageBox.Show("Task successfully assigned!", "Success", MessageBoxButton.OK);
                    NavService.ToManageMemberTasksPage(Member);
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Task not assigned", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            catch (OverflowException ex)
            {
                MessageBox.Show("Entered value is too big", "Overflow error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (FormatException)
            {
                MessageBox.Show("Please fill in a whole number for reward", "Overflow error", MessageBoxButton.OK, MessageBoxImage.Warning); // zou normaal gezien niet mogen voorvallen
            }
            catch (Exception ex) //Voor als iets niet in orde met velden
            {
                MessageBox.Show("Something unexpected went wrong: " + ex.Message, "Unexpected error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            
        }

        private void AssignTask_Click(object sender, RoutedEventArgs e) //voor reeds bestaande taak te hergebruiken
        {
            try
            {
                if (dpDeadlineLibrary.SelectedDate == null) throw new ArgumentException("No deadline selected!");
                if (lsvTaskLibrary.SelectedItem == null) throw new ArgumentException("No task selected!");
                Classes.Task newTask = lsvTaskLibrary.SelectedItem as Classes.Task;
                if (newTask != null)
                {
                    NewAssignedTask = TaskService.CreateTaskInstance(newTask, dpDeadlineLibrary.SelectedDate.Value, Member);
                    MessageBox.Show("Task successfully assigned!", "Success", MessageBoxButton.OK);
                    NavService.ToManageMemberTasksPage(Member);
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Task not assigned", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            catch (Exception ex) //Voor als iets niet in orde met velden
            {
                MessageBox.Show("Something unexpected went wrong: " + ex.Message, "Unexpected error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }



        private static readonly Regex _regex = new("^[0-9]+$");
        private void NumberOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_regex.IsMatch(e.Text);
        }

        private void NumberOnly_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                string text = (string)e.DataObject.GetData(DataFormats.Text);
                if (!_regex.IsMatch(text))
                    e.CancelCommand();
            }
            else
                e.CancelCommand();
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            NavService.ToManageMemberTasksPage(Member);
        }
    }
}
