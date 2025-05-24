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


namespace donely_Inspilab.Pages.Task
{
    /// <summary>
    /// Interaction logic for ManageTasksPage.xaml
    /// </summary>
    public partial class TaskLibraryPage : Page
    {
        public TaskLibraryPage()
        {
            InitializeComponent();
            LoadLibrary();
            //lsvTaskLibrary.ItemsSource = GroupState.LoadedGroup.Tasks;
        }
        List<Classes.Task> taskList = [];
        private void LoadLibrary()
        {
            try
            {
                taskList = TaskService.GetGroupDefinitions(GroupState.LoadedGroup.Id);
                lsvTaskLibrary.ItemsSource = taskList;
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
        }

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ToggleTask_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
