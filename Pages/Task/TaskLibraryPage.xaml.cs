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
    /// Interaction logic for TaskLibraryPage.xaml
    /// </summary>
    public partial class TaskLibraryPage : Page
    {
        public TaskLibraryPage()
        {
            InitializeComponent();
            LoadLibrary();
            //lsvTaskLibrary.ItemsSource = GroupState.LoadedGroup.Tasks;
        }

        private void LoadLibrary()
        {
            try
            {
                GroupState.LoadedGroup.TaskDefinitions = TaskService.GetGroupDefinitions(GroupState.LoadedGroup.Id);
                lsvTaskLibrary.ItemsSource = GroupState.LoadedGroup.TaskDefinitions;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong loading tasks");
            }
        }


        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            CreateNewTaskWindow newTaskWindow = new CreateNewTaskWindow();
            var result = newTaskWindow.ShowDialog();
            if (result==true && newTaskWindow.NewTask != null)
            {
                GroupState.LoadedGroup.TaskDefinitions.Add(newTaskWindow.NewTask);
                lsvTaskLibrary.Items.Refresh();
            }
        }

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            if (lsvTaskLibrary.SelectedItem == null)
            {
                MessageBox.Show("Please select a task to edit.", "Edit Task", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Classes.Task selectedTask = (Classes.Task)lsvTaskLibrary.SelectedItem;
            EditTaskWindow editWindow = new EditTaskWindow(selectedTask);
            var result = editWindow.ShowDialog();

            if (result == true && editWindow.EditedTask != null)
            {
                int index = GroupState.LoadedGroup.TaskDefinitions.IndexOf(selectedTask);
                GroupState.LoadedGroup.TaskDefinitions[index] = editWindow.EditedTask;
                lsvTaskLibrary.Items.Refresh();
            }
        }

        private void ToggleTask_Click(object sender, RoutedEventArgs e)
        {
            //Toggle recurring tasks --> disabled = auto-assign will stop
            Classes.Task selectedTask = (Classes.Task)lsvTaskLibrary.SelectedItem;
            bool newActiveState = !selectedTask.IsActive; // Togle Status
            try
            {
                TaskService.ToggleTaskIsActive(selectedTask.Id, newActiveState);
                selectedTask.IsActive = newActiveState;
                lsvTaskLibrary.Items.Refresh();
                btnToggle.Content = newActiveState ? "Deactivate" : "Activate";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update task status: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete this task?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No) 
                return;
            try
            {
                Classes.Task selectedTask = (Classes.Task)lsvTaskLibrary.SelectedItem;
                TaskService.ToggleTaskIsActive(selectedTask.Id, false); //ervoor zorgen dat recurring niet verder herhaald voor verwijderde taken
                TaskService.SoftDeleteTask(selectedTask.Id);
                GroupState.LoadedGroup.TaskDefinitions.Remove(selectedTask);
                lsvTaskLibrary.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }//
         //als Task Instances op punt staat --> Check doen of er een actieve instance is (dus != fail/success) -> zo ja, dan kan deze nog niet verwijderd worden. Voor recurring ook check Active/Not Active



        //Event voor als selectie ListView veranderd --> toggle button hangt hiervan af:
        private void lsvTaskLibrary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lsvTaskLibrary.SelectedItem is Classes.Task selectedTask)
            {
                // Disable button if frequency is Once (one-time)
                btnToggle.IsEnabled = selectedTask.Frequency != TaskFrequency.Once; //Activatie kan enkel voor Recurring Tasks

                btnToggle.Content = selectedTask.IsActive ? "Deactivate" : "Activate"; // Past content aan naargelang task geactiveerd of gedeactiveerd kan worden
            }
            else
            {
                btnToggle.IsEnabled = false;
                btnToggle.Content = "Activate";
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            NavService.ToGroupOwnerPage();
        }
    }
}
